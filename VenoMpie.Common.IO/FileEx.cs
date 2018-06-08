using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO
{
    /// <summary>
    /// Extended File/Directory Functions
    /// </summary>
    /// <remarks>
    /// I was getting sick of having to copy/move files using System.IO with no indication of how far I am especially using while using Tasks/WPF
    /// Can't believe .NET functions still don't have progress callbacks, so here's functions that goes into the Windows Kernel to do that
    /// </remarks>
    public class FileEx
    {
        /// <summary>
        /// Same as System.IO.File.Copy apart from it's asynchronous
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="overwrite">Overwrite the existing file.
        /// If FALSE is specified and the file exists, this function will throw an exception
        /// </param>
        /// <returns>Task=>bool</returns>
        /// <remarks>I was already implementing all the Kernel Functions for Progress so I might as well do this one as well</remarks>
        public static Task<bool> CopyAsync(string sourceFileName, string destFileName, bool overwrite)
        {
            return Task.Run(() =>
            {
                return CopyFile(sourceFileName, destFileName, !overwrite);
            });
        }
        /// <summary>
        /// Copies a file/folder asynchronously which can be cancelled with a CancellationToken
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="token">Cancellation Token used to cancel the task</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> CopyAsync(string sourceFileName, string destFileName, CancellationToken token) => CopyAsync(sourceFileName, destFileName, token, EmptyCopyProgressHandler);
        /// <summary>
        /// Copies a file/folder asynchronously while invoking a Sytem.IProgress function to return a value between 0 and 100 (mostly used for Progressbars)
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="progress">IProgress callback function</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> CopyAsync(string sourceFileName, string destFileName, IProgress<double> progress) => CopyAsync(sourceFileName, destFileName, CancellationToken.None, progress);
        /// <summary>
        /// Copies a file/folder asynchronously while invoking a Sytem.IProgress function to return a value between 0 and 100 and can be cancelled
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="token">Cancellation Token used to cancel the task</param>
        /// <param name="progress">IProgress callback function</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> CopyAsync(string sourceFileName, string destFileName, CancellationToken token, IProgress<double> progress)
        {
            CopyProgressRoutine copyProgressHandler = GetIProgressHandler(progress);
            return CopyAsync(sourceFileName, destFileName, token, copyProgressHandler);
        }
        /// <summary>
        /// Copies a file/folder asynchronously which can be cancelled and exposes the CopyProgressRoutine for handling bytes transferred and other properties
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="token">Cancellation Token used to cancel the task</param>
        /// <param name="copyProgressHandler">CopyProgressRoutine callback function</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> CopyAsync(string sourceFileName, string destFileName, CancellationToken token, CopyProgressRoutine copyProgressHandler)
        {
            int pbCancel = 0;

            token.ThrowIfCancellationRequested();
            var ctr = token.Register(() => pbCancel = 1);
            return Task.Run(() =>
            {
                try
                {
                    return CopyFileEx(sourceFileName, destFileName, copyProgressHandler, IntPtr.Zero, ref pbCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);
                }
                finally
                {
                    ctr.Dispose();
                }
            }, token);
        }

        /// <summary>
        /// Same as System.IO.File.Move apart from it's asynchronous
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <returns>Task=>bool</returns>
        /// <remarks>I was already implementing all the Kernel Functions for Progress so I might as well do this one as well</remarks>
        public static Task<bool> MoveAsync(string sourceFileName, string destFileName)
        {
            return Task.Run(() =>
            {
                return MoveFile(sourceFileName, destFileName);
            });
        }
        /// <summary>
        /// Moves a file/folder asynchronously while invoking a Sytem.IProgress function to return a value between 0 and 100 (mostly used for Progressbars)
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="progress">IProgress callback function</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> MoveAsync(string sourceFileName, string destFileName, IProgress<double> progress)
        {
            CopyProgressRoutine copyProgressHandler = GetIProgressHandler(progress);
            return MoveAsync(sourceFileName, destFileName, copyProgressHandler);
        }
        /// <summary>
        /// Moves a file/folder asynchronously which exposes the CopyProgressRoutine for handling bytes transferred and other properties
        /// </summary>
        /// <param name="sourceFileName">Full Path of the Source File/Folder</param>
        /// <param name="destFileName">Full Path of the Destination File/Folder</param>
        /// <param name="copyProgressHandler">CopyProgressRoutine callback function</param>
        /// <returns>Task=>bool</returns>
        public static Task<bool> MoveAsync(string sourceFileName, string destFileName, CopyProgressRoutine copyProgressHandler)
        {
            return Task.Run(() =>
            {
                return MoveFileWithProgress(sourceFileName, destFileName, copyProgressHandler, IntPtr.Zero, MoveFileFlags.MOVEFILE_COPY_ALLOWED | MoveFileFlags.MOVEFILE_REPLACE_EXISTING | MoveFileFlags.MOVEFILE_WRITE_THROUGH);
            });
        }

        private static CopyProgressResult EmptyCopyProgressHandler(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData) => CopyProgressResult.PROGRESS_CONTINUE;
        private static CopyProgressRoutine GetIProgressHandler(IProgress<double> progress)
        {
            CopyProgressRoutine copyProgressHandler;
            if (progress != null)
            {
                copyProgressHandler = (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, hSourceFile, hDestinationFile, lpData) =>
                {
                    progress.Report((double)totalBytesTransferred / totalFileSize * 100);
                    return CopyProgressResult.PROGRESS_CONTINUE;
                };
            }
            else
            {
                copyProgressHandler = EmptyCopyProgressHandler;
            }
            return copyProgressHandler;
        }

        #region DLL Import
        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="lpExistingFileName">The name of an existing file.</param>
        /// <param name="lpNewFileName">The name of the new file.</param>
        /// <param name="bFailIfExists">If this parameter is TRUE and the new file specified by lpNewFileName already exists, the function fails.
        /// If this parameter is FALSE and the new file already exists, the function overwrites the existing file and succeeds.
        /// </param>
        /// <returns></returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa363851(v=vs.85).aspx"/>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

        /// <summary>
        /// Copies an existing file to a new file, notifying the application of its progress through a callback function.
        /// </summary>
        /// <param name="lpExistingFileName">The name of an existing file.</param>
        /// <param name="lpNewFileName">The name of the new file.</param>
        /// <param name="lpProgressRoutine">The address of a callback function of type LPPROGRESS_ROUTINE that is called each time another portion of the file has been copied.
        /// This parameter can be NULL.
        /// </param>
        /// <param name="lpData">The argument to be passed to the callback function.
        /// This parameter can be NULL.
        /// </param>
        /// <param name="pbCancel">If this flag is set to TRUE during the copy operation, the operation is canceled.
        /// Otherwise, the copy operation will continue to completion.
        /// </param>
        /// <param name="dwCopyFlags">Flags that specify how the file is to be copied.</param>
        /// <returns>bool</returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa363852(v=vs.85).aspx"/>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel, CopyFileFlags dwCopyFlags);

        /// <summary>
        /// Moves an existing file or a directory, including its children.
        /// </summary>
        /// <param name="lpExistingFileName">The current name of the file or directory on the local computer.</param>
        /// <param name="lpNewFileName">The new name for the file or directory.
        /// The new name must not already exist.
        /// A new file may be on a different file system or drive. A new directory must be on the same drive.
        /// </param>
        /// <returns>bool</returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365239(v=vs.85).aspx"/>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MoveFile(string lpExistingFileName, string lpNewFileName);

        /// <summary>
        /// Moves an existing file or directory, including its children, with various move options.
        /// </summary>
        /// <param name="lpExistingFileName">The current name of the file or directory on the local computer.
        /// If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT, the file cannot exist on a remote share, because delayed operations are performed before the network is available.
        /// </param>
        /// <param name="lpNewFileName">The new name of the file or directory on the local computer.
        /// When moving a file, the destination can be on a different file system or volume.
        /// If the destination is on another drive, you must set the MOVEFILE_COPY_ALLOWED flag in dwFlags.
        /// When moving a directory, the destination must be on the same drive.
        /// If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT and lpNewFileName is NULL, MoveFileEx registers the lpExistingFileName file to be deleted when the system restarts.
        /// If lpExistingFileName refers to a directory, the system removes the directory at restart only if the directory is empty.
        /// </param>
        /// <param name="dwFlags">The move options.</param>
        /// <returns>bool</returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365240(v=vs.85).aspx"/>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);

        /// <summary>
        /// Moves a file or directory, including its children. You can provide a callback function that receives progress notifications.
        /// </summary>
        /// <param name="lpExistingFileName">The name of the existing file or directory on the local computer.
        /// If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT, the file cannot exist on a remote share because delayed operations are performed before the network is available.
        /// </param>
        /// <param name="lpNewFileName">The new name of the file or directory on the local computer.
        /// When moving a file, lpNewFileName can be on a different file system or volume.
        /// If lpNewFileName is on another drive, you must set the MOVEFILE_COPY_ALLOWED flag in dwFlags.
        /// When moving a directory, lpExistingFileName and lpNewFileName must be on the same drive.
        /// If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT and lpNewFileName is NULL, MoveFileWithProgress registers lpExistingFileName to be deleted when the system restarts.
        /// The function fails if it cannot access the registry to store the information about the delete operation.
        /// If lpExistingFileName refers to a directory, the system removes the directory at restart only if the directory is empty
        /// </param>
        /// <param name="lpProgressRoutine">A pointer to a CopyProgressRoutine callback function that is called each time another portion of the file has been moved.
        /// The callback function can be useful if you provide a user interface that displays the progress of the operation.
        /// This parameter can be NULL
        /// </param>
        /// <param name="lpData">An argument to be passed to the CopyProgressRoutine callback function.
        /// This parameter can be NULL.
        /// </param>
        /// <param name="dwFlags">The move options.</param>
        /// <returns>bool</returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365242(v=vs.85).aspx"/>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MoveFileWithProgress(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, MoveFileFlags dwFlags);

        /// <summary>
        /// An application-defined callback function used with the CopyFileEx, MoveFileTransacted, and MoveFileWithProgress functions.
        /// It is called when a portion of a copy or move operation is completed.
        /// The LPPROGRESS_ROUTINE type defines a pointer to this callback function.
        /// CopyProgressRoutine is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="totalFileSize">The total size of the file, in bytes.</param>
        /// <param name="totalBytesTransferred">The total number of bytes transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="streamSize">The total size of the current file stream, in bytes.</param>
        /// <param name="streamBytesTransferred">The total number of bytes in the current stream that have been transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="dwStreamNumber">A handle to the current stream.
        /// The first time CopyProgressRoutine is called, the stream number is 1.
        /// </param>
        /// <param name="dwCallbackReason">The reason that CopyProgressRoutine was called.</param>
        /// <param name="hSourceFile">A handle to the source file.</param>
        /// <param name="hDestinationFile">A handle to the destination file</param>
        /// <param name="lpData">Argument passed to CopyProgressRoutine by CopyFileEx, MoveFileTransacted, or MoveFileWithProgress.</param>
        /// <returns>CopyProgressResult</returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa363854(v=vs.85).aspx"/>
        public delegate CopyProgressResult CopyProgressRoutine(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        public enum CopyProgressResult : uint
        {
            /// <summary>
            /// Continue the copy operation.
            /// </summary>
            PROGRESS_CONTINUE = 0,
            /// <summary>
            /// Cancel the copy operation and delete the destination file.
            /// </summary>
            PROGRESS_CANCEL = 1,
            /// <summary>
            /// Stop the copy operation. It can be restarted at a later time.
            /// </summary>
            PROGRESS_STOP = 2,
            /// <summary>
            /// Continue the copy operation, but stop invoking CopyProgressRoutine to report progress.
            /// </summary>
            PROGRESS_QUIET = 3
        }

        public enum CopyProgressCallbackReason : uint
        {
            /// <summary>
            /// Another part of the data file was copied.
            /// </summary>
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            /// <summary>
            /// Another stream was created and is about to be copied.
            /// This is the callback reason given when the callback routine is first invoked.
            /// </summary>
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        public enum CopyFileFlags : uint
        {
            /// <summary>
            /// The copy operation fails immediately if the target file already exists.
            /// </summary>
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            /// <summary>
            /// Progress of the copy is tracked in the target file in case the copy fails.
            /// The failed copy can be restarted at a later time by specifying the same values for lpExistingFileName and lpNewFileName as those used in the call that failed.
            /// This can significantly slow down the copy operation as the new file may be flushed multiple times during the copy operation.
            /// </summary>
            COPY_FILE_RESTARTABLE = 0x00000002,
            /// <summary>
            /// The file is copied and the original file is opened for write access.
            /// </summary>
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            /// <summary>
            /// An attempt to copy an encrypted file will succeed even if the destination copy cannot be encrypted.
            /// </summary>
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008,
            /// <summary>
            /// If the source file is a symbolic link, the destination file is also a symbolic link pointing to the same file that the source symbolic link is pointing to.
            /// </summary>
            /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported.</remarks>
            COPY_FILE_COPY_SYMLINK = 0x00000800,
            /// <summary>
            /// The copy operation is performed using unbuffered I/O, bypassing system I/O cache resources. Recommended for very large file transfers.
            /// </summary>
            /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported.</remarks>
            COPY_FILE_NO_BUFFERING = 0x00001000
        }

        [Flags]
        public enum MoveFileFlags : uint
        {
            /// <summary>
            /// If a file named lpNewFileName exists, the function replaces its contents with the contents of the lpExistingFileName file.
            /// </summary>
            /// <remarks>This value cannot be used if lpNewFileName or lpExistingFileName names a directory.</remarks>
            MOVEFILE_REPLACE_EXISTING = 0x1,
            /// <summary>
            /// If the file is to be moved to a different volume, the function simulates the move by using the CopyFile and DeleteFile functions.
            /// If the file is successfully copied to a different volume and the original file is unable to be deleted, the function succeeds leaving the source file intact.
            /// </summary>
            /// <remarks>This value cannot be used with MOVEFILE_DELAY_UNTIL_REBOOT.</remarks>
            MOVEFILE_COPY_ALLOWED = 0x2,
            /// <summary>
            /// The system does not move the file until the operating system is restarted.
            /// The system moves the file immediately after AUTOCHK is executed, but before creating any paging files.
            /// Consequently, this parameter enables the function to delete paging files from previous startups.
            /// </summary>
            /// <remarks>
            /// This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.
            /// This value cannot be used with MOVEFILE_COPY_ALLOWED.
            /// </remarks>
            MOVEFILE_DELAY_UNTIL_REBOOT = 0x4,
            /// <summary>
            /// The function does not return until the file has actually been moved on the disk.
            /// Setting this value guarantees that a move performed as a copy and delete operation is flushed to disk before the function returns.
            /// The flush occurs at the end of the copy operation.
            /// </summary>
            /// <remarks>This value has no effect if MOVEFILE_DELAY_UNTIL_REBOOT is set.</remarks>
            MOVEFILE_WRITE_THROUGH = 0x8,
            ///// <summary>
            ///// Reserved for future use.
            ///// </summary>
            //MOVEFILE_CREATE_HARDLINK = 0x10,
            /// <summary>
            /// The function fails if the source file is a link source, but the file cannot be tracked after the move.
            /// This situation can occur if the destination is a volume formatted with the FAT file system.
            /// </summary>
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20,
        }
        #endregion
    }
}
