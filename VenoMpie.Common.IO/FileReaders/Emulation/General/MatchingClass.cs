using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.General
{
    public enum MatchOptions
    {
        MatchBasic = 0,
        MatchBasicAndFileDate = 1
    }
    public enum MatchLevel
    {
        PerfectMatch = 0,
        NoMatch_FileNameExtension = 1,
        NoMatch_FileNameExtensionDate = 2,
        Match_Date_NoMatch_FileNameExtension = 3,
        Match_FileNameExtension_NoMatch_Date = 4,
        Match_FileNameExtension_NoMatch_CRC = 5,
    }
    #region Entity Classes
    public class MatchingEntity
    {
        public Tuple<string, string> SourceTarget { get; set; }
        public decimal SourceFilesCount { get; set; } //This will be the Original File
        public decimal TargetFilesCount { get; set; } //Game Entry to Match
        public decimal PercentageMatch { get; set; }
        //public Dictionary<FileEntryBase, MatchLevel> MatchedFiles { get; set; }
        public List<FileEntryBase> MatchedFiles { get; set; }
        public List<FileEntryBase> NonMatchedFiles { get; set; }
        public List<FileEntryBase> ExpectedFiles { get; set; }
        public List<FileEntryBase> IgnoredFiles { get; set; }

        public MatchingEntity()
        {
            //MatchedFiles = new Dictionary<FileEntryBase, MatchLevel>();
            MatchedFiles = new List<FileEntryBase>();
            NonMatchedFiles = new List<FileEntryBase>();
            ExpectedFiles = new List<FileEntryBase>();
            IgnoredFiles = new List<FileEntryBase>();
        }
    }
    #endregion
    /// <summary>
    /// This class is for matching files based on CRC/Filename/Extension/etc. Used for matching sets of files rather than 1 on 1
    /// </summary>
    public class Matching
    {
        #region File Matching
        public MatchingEntity MatchBasic(KeyValuePair<string, FileEntryBase> source, Dictionary<string, FileEntryBase> target, MatchOptions options)
        {
            foreach (var targetItem in target)
            {
                MatchingEntity matched = new MatchingEntity() { SourceFilesCount = 1, TargetFilesCount = 1 };
                if (targetItem.Value.Crc32.ToUpper() == source.Value.Crc32.ToUpper())
                {
                    KeyValuePair<FileEntryBase, MatchLevel> entry = MatchFileEntry(source.Value, targetItem.Value, options);
                    matched.PercentageMatch = 100;
                    matched.SourceTarget = new Tuple<string, string>(source.Key, targetItem.Key);
                    //matched.MatchedFiles.Add(entry.Key, entry.Value);
                    matched.MatchedFiles.Add(entry.Key);
                    return matched;
                }
            }
            return null;
        }
        public List<MatchingEntity> MatchBasic(Dictionary<string, FileEntryBase> source, Dictionary<string, FileEntryBase> target, MatchOptions options)
        {
            List<MatchingEntity> retValue = new List<MatchingEntity>();
            Parallel.ForEach(source, item =>
            {
                MatchingEntity entity = MatchBasic(item, target, options);
                if (entity != null) retValue.Add(entity);
            });
            return retValue;
        }
        #region PseudoMethods
        public MatchingEntity MatchBasic(KeyValuePair<string, FileEntryBase> source, Dictionary<FileInfo, FileEntryBase> target, MatchOptions options)
        {
            return MatchBasic(source, target.ToDictionary(a => a.Key.FullName, a => a.Value), options);
        }
        public MatchingEntity MatchBasic(KeyValuePair<FileInfo, FileEntryBase> source, Dictionary<string, FileEntryBase> target, MatchOptions options)
        {
            return MatchBasic(new KeyValuePair<string, FileEntryBase>(source.Key.FullName, source.Value), target, options);
        }
        public List<MatchingEntity> MatchBasic(Dictionary<FileInfo, FileEntryBase> source, Dictionary<string, FileEntryBase> target, MatchOptions options)
        {
            return MatchBasic(source.ToDictionary(a => a.Key.FullName, a => a.Value), target, options);
        }
        public List<MatchingEntity> MatchBasic(Dictionary<string, FileEntryBase> source, Dictionary<FileInfo, FileEntryBase> target, MatchOptions options)
        {
            return MatchBasic(source, target.ToDictionary(a => a.Key.FullName, a => a.Value), options);
        }
        #endregion
        #endregion
        #region Zip File Matching
        public MatchingEntity MatchBasicZip(KeyValuePair<string, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, MatchOptions options, int minimumSize = 10)
        {
            Dictionary<MatchingEntity, decimal> MatchedGames = new Dictionary<MatchingEntity, decimal>();

            bool fileFound = false;

            //foreach (var targetItem in target)
            Parallel.ForEach(target, targetItem =>
            {
                MatchingEntity matched = new MatchingEntity() { SourceTarget = new Tuple<string, string>(source.Key, targetItem.Key), SourceFilesCount = source.Value.Count, TargetFilesCount = targetItem.Value.Count };

                fileFound = false;
                foreach (var targetFile in targetItem.Value)//.Where(a => minimumSize == -1 || a.Size > minimumSize))
                //Parallel.ForEach(targetItem.Value, targetFile =>
                {
                    if (targetFile.Crc32 != "00000000")
                    {
                        foreach (var sourceFile in source.Value)//.Where(a => minimumSize == -1 || a.Size > minimumSize))
                        {
                            //if (sourceFile.IsIgnored)
                            //{
                            //    matched.IgnoredFiles.Add(sourceFile);
                            //    fileFound = true;
                            //    break;
                            //}
                            //else if ((targetFile.GetHashCode_CRC == sourceFile.GetHashCode_CRC && targetFile.GetHashCode_NameExtension == sourceFile.GetHashCode_NameExtension)
                            //    || targetFile.GetHashCode_NameExtension == sourceFile.GetHashCode_NameExtension)
                            //{
                            //    matched.MatchedFiles.Add(sourceFile);
                            //    fileFound = true;
                            //    break;
                            //}
                        }
                    }
                    if (!fileFound) matched.ExpectedFiles.Add(targetFile);
                }
                //);
                if (matched.MatchedFiles.Count > 0)
                {
                    //matched.NonMatchedFiles = source.Value.Where(a => !matched.MatchedFiles.ContainsKey(a)).ToList();
                    matched.NonMatchedFiles = source.Value.Where(a => !matched.MatchedFiles.Contains(a)).ToList();
                    //This is the best way of matching the to the correct file that I could find. If there's a better way, change it :P
                    //I wanted to go with the Not Matched files counting to more than 100 but then it starts throwing off perfect matches so I would rather stick with not being 100% even though the zip has a lot of superfluous data e.g. nfo's etc.
                    //If it's got crap files in, remove it from the zip to get it to 100 :P
                    matched.PercentageMatch = (matched.MatchedFiles.Count == 0) ? 0 : Convert.ToDecimal(matched.MatchedFiles.Count) / Math.Max(matched.TargetFilesCount, matched.SourceFilesCount) * 100;
                    //if (matched.PercentageMatch > 30)
                    //{
                        lock (MatchedGames)
                        {
                            MatchedGames.Add(matched, matched.PercentageMatch);
                        }
                    //}
                }
            }
            );
            return MatchedGames.OrderByDescending(a => a.Value).FirstOrDefault().Key;
        }
        public MatchingEntity MatchBasicZip(KeyValuePair<string, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, List<FileEntryBase> ignoreFiles, MatchOptions options, int minimumSize = 10)
        {
            UpdateIgnoredIndicator(ref target, ignoreFiles);
            return MatchBasicZip(source, target, options);
        }
        public List<MatchingEntity> MatchBasicZip(Dictionary<string, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, MatchOptions options)
        {
            List<MatchingEntity> retValue = new List<MatchingEntity>();
            Parallel.ForEach(source, item =>
            {
                MatchingEntity entity = MatchBasicZip(item, target, options);
                if (entity != null) retValue.Add(entity);
            });
            return retValue;
        }
        public List<MatchingEntity> MatchBasicZip(Dictionary<string, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, List<FileEntryBase> ignoreFiles, MatchOptions options)
        {
            UpdateIgnoredIndicator(ref source, ignoreFiles);
            UpdateIgnoredIndicator(ref target, ignoreFiles);
            return MatchBasicZip(source, target, options);
        }
        #region PseudoMethods
        public MatchingEntity MatchBasicZip(KeyValuePair<string, List<FileEntryBase>> source, Dictionary<FileInfo, List<FileEntryBase>> target, MatchOptions options) => MatchBasicZip(source, target.ToDictionary(a => a.Key.FullName, a => a.Value), options);
        public MatchingEntity MatchBasicZip(KeyValuePair<string, List<FileEntryBase>> source, Dictionary<FileInfo, List<FileEntryBase>> target, List<FileEntryBase> ignoreFiles, MatchOptions options) => MatchBasicZip(source, target.ToDictionary(a => a.Key.FullName, a => a.Value), ignoreFiles, options);
        public MatchingEntity MatchBasicZip(KeyValuePair<FileInfo, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, MatchOptions options) => MatchBasicZip(new KeyValuePair<string, List<FileEntryBase>>(source.Key.FullName, source.Value), target, options);
        public MatchingEntity MatchBasicZip(KeyValuePair<FileInfo, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, List<FileEntryBase> ignoreFiles, MatchOptions options) => MatchBasicZip(new KeyValuePair<string, List<FileEntryBase>>(source.Key.FullName, source.Value), target, ignoreFiles, options);
        public List<MatchingEntity> MatchBasicZip(Dictionary<FileInfo, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, MatchOptions options) => MatchBasicZip(source.ToDictionary(a => a.Key.FullName, a => a.Value), target, options);
        public List<MatchingEntity> MatchBasicZip(Dictionary<FileInfo, List<FileEntryBase>> source, Dictionary<string, List<FileEntryBase>> target, List<FileEntryBase> ignoreFiles, MatchOptions options) => MatchBasicZip(source.ToDictionary(a => a.Key.FullName, a => a.Value), target, ignoreFiles, options);
        #endregion
        #endregion
        #region Helper Functions
        private KeyValuePair<FileEntryBase, MatchLevel> MatchFileEntry(FileEntryBase source, FileEntryBase target, MatchOptions options)
        {
            switch (options)
            {
                case MatchOptions.MatchBasic:
                    return new KeyValuePair<FileEntryBase, MatchLevel>(source,
                        (source.Name.ToUpper() == target.Name.ToUpper() && source.Extension.ToUpper() == target.Extension.ToUpper()) ? MatchLevel.PerfectMatch : MatchLevel.NoMatch_FileNameExtension);
                case MatchOptions.MatchBasicAndFileDate:
                    return new KeyValuePair<FileEntryBase, MatchLevel>(source,
                        (source.Name.ToUpper() == target.Name.ToUpper() && source.Extension.ToUpper() == target.Extension.ToUpper() && source.FileDate == target.FileDate) ? MatchLevel.PerfectMatch :
                        (source.Name.ToUpper() == target.Name.ToUpper() && source.Extension.ToUpper() == target.Extension.ToUpper()) ? MatchLevel.Match_FileNameExtension_NoMatch_Date :
                        (source.FileDate == target.FileDate) ? MatchLevel.Match_Date_NoMatch_FileNameExtension : MatchLevel.NoMatch_FileNameExtensionDate);
                default:
                    return new KeyValuePair<FileEntryBase, MatchLevel>(source,
                        (source.Name.ToUpper() == target.Name.ToUpper() && source.Extension.ToUpper() == target.Extension.ToUpper()) ? MatchLevel.PerfectMatch : MatchLevel.NoMatch_FileNameExtension);
            }
        }
        private bool MatchIgnoreFile(List<FileEntryBase> ignoreFiles, FileEntryBase file)
        {
            foreach (var ignoreFile in ignoreFiles)
            {
                //if (
                //    (ignoreFile.GetHashCode_Name == file.GetHashCode_Name || ignoreFile.Name == "*")
                //    &&
                //    (ignoreFile.GetHashCode_Extension == file.GetHashCode_Extension || ignoreFile.Extension == "*")
                //    &&
                //    (ignoreFile.GetHashCode_CRC == file.GetHashCode_CRC || ignoreFile.Crc32 == "*")
                //    )
                //    return true;
            }

            return false;
        }

        //private List<FileEntryBase> RemoveIgnoreFileFromList(List<FileEntryBase> listToRemove, List<FileEntryBase> ignoreFiles)
        //{
        //    return listToRemove.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList();
        //}
        //private KeyValuePair<string, List<FileEntryBase>> RemoveIgnoreFileFromList(KeyValuePair<string, List<FileEntryBase>> listToRemove, List<FileEntryBase> ignoreFiles)
        //{
        //    return new KeyValuePair<string,List<FileEntryBase>>(listToRemove.Key, listToRemove.Value.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList());
        //}
        //private Dictionary<string, List<FileEntryBase>> RemoveIgnoreFileFromList(Dictionary<string, List<FileEntryBase>> listToRemove, List<FileEntryBase> ignoreFiles)
        //{
        //    Dictionary<string, List<FileEntryBase>> retValue = new Dictionary<string, List<FileEntryBase>>();
        //    foreach (var item in listToRemove)
        //    {
        //        retValue.Add(item.Key, item.Value.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList());
        //    }
        //    return retValue;
        //}
        private void UpdateIgnoredIndicator(ref List<FileEntryBase> listToRemove, List<FileEntryBase> ignoreFiles)
        {
            listToRemove = listToRemove.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList();
        }
        private void UpdateIgnoredIndicator(ref Dictionary<string, List<FileEntryBase>> listToRemove, List<FileEntryBase> ignoreFiles)
        {
            Dictionary<string, List<FileEntryBase>> retValue = new Dictionary<string, List<FileEntryBase>>();
            foreach (var item in listToRemove)
            {
                retValue.Add(item.Key, item.Value.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList());
            }
            listToRemove = retValue;
        }
        private void UpdateIgnoredIndicator(ref Dictionary<FileInfo, List<FileEntryBase>> listToRemove, List<FileEntryBase> ignoreFiles)
        {
            Dictionary<FileInfo, List<FileEntryBase>> retValue = new Dictionary<FileInfo, List<FileEntryBase>>();
            foreach (var item in listToRemove)
            {
                retValue.Add(item.Key, item.Value.Where(f => !MatchIgnoreFile(ignoreFiles, f)).ToList());
            }
            listToRemove = retValue;
        }
        #endregion
    }
}
