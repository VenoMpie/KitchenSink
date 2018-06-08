using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Drawing.ConsoleDrawing
{
    public class BSAVED
    {
        /// <summary>
        /// Get the Console Background Color from a byte
        /// </summary>
        /// <param name="fromByte"></param>
        /// <returns>Int value that can be casted to ConsoleColor</returns>
        /// <remarks>Uses the standard DOS method of using the second byte to determine the color</remarks>
        public static int GetBackgroundColor(byte fromByte)
        {
            return fromByte >> 4;
        }
        /// <summary>
        /// Get the Console Foreground Color from a byte
        /// </summary>
        /// <param name="fromByte"></param>
        /// <returns>Int value that can be casted to ConsoleColor</returns>
        /// <remarks>Uses the standard DOS method of using the second byte to determine the color</remarks>
        public static int GetForegroundColor(byte fromByte)
        {
            return fromByte & 0x0F;
        }
        /// <summary>
        /// Display a B800 Screen on the console
        /// </summary>
        /// <param name="bytes">Array of 4000 bytes</param>
        public static void DisplayBSAVEDScreen(byte[] bytes) => DisplayBSAVEDScreen(bytes, 80, Encoding.GetEncoding(437));

        /// <summary>
        /// Display a B800 Screen on the console
        /// </summary>
        /// <param name="bytes">Array of 4000 bytes</param>
        /// <param name="widthOffset">Length of the screen</param>
        /// <remarks>
        /// Don't use this unless you are sure about the size.
        /// The Standard Width/Height is 80x25 for a B800 screen and those are our defaults when only passing the bytes
        /// </remarks>
        /// <see cref="https://en.wikipedia.org/wiki/BSAVE_(bitmap_format)"/>
        public static void DisplayBSAVEDScreen(byte[] bytes, int widthOffset) => DisplayBSAVEDScreen(bytes, widthOffset, Encoding.GetEncoding(437));

        /// <summary>
        /// Display a B800 Screen on the console
        /// </summary>
        /// <param name="bytes">Array of 4000 bytes</param>
        /// <param name="widthOffset">Length of the screen</param>
        /// <param name="encoding">Encoding to use</param>
        /// <remarks>
        /// Also don't use this unless you are sure about the encoding.
        /// DOS uses the IBM PC Character Set (CP437) so we default to that when only passing bytes
        /// </remarks>
        /// <see cref="https://en.wikipedia.org/wiki/BSAVE_(bitmap_format)"/>
        /// <seealso cref="https://en.wikipedia.org/wiki/Code_page_437"/>
        public static void DisplayBSAVEDScreen(byte[] bytes, int widthOffset, Encoding encoding)
        {
            Console.Clear();
            int currentWidth = 0;
            if (bytes.Length == 0 || bytes.Length % 2 != 0) throw new Exception("Invalid byte length");

            for (int i = 0; i < bytes.Length; i += 2)
            {
                char[] character = encoding.GetString(new byte[] { bytes[i] }).ToCharArray();
                currentWidth += 2;
                Console.BackgroundColor = (ConsoleColor)GetBackgroundColor(bytes[i + 1]);
                Console.ForegroundColor = (ConsoleColor)GetForegroundColor(bytes[i + 1]);
                Console.Write(character);
                if ((currentWidth / 2) >= widthOffset)
                {
                    currentWidth = 0;
                    if (i != bytes.Length - 2) Console.WriteLine();
                }
            }
        }

        #region Text Drawing Methods
        public static void DrawText(char character, byte color, int repeats, bool writeLine)
        {
            DrawText(character, (ConsoleColor)GetBackgroundColor(color), (ConsoleColor)GetForegroundColor(color), repeats, writeLine, Console.CursorLeft, Console.CursorTop);
        }
        public static void DrawText(char character, byte color, int repeats, bool writeLine, int x, int y)
        {
            DrawText(character, (ConsoleColor)GetBackgroundColor(color), (ConsoleColor)GetForegroundColor(color), repeats, writeLine, x, y);
        }
        public static void DrawText(char character, byte color, bool writeLine)
        {
            DrawText(character, (ConsoleColor)GetBackgroundColor(color), (ConsoleColor)GetForegroundColor(color), writeLine, Console.CursorLeft, Console.CursorTop);
        }
        public static void DrawText(char character, byte color, bool writeLine, int x, int y)
        {
            DrawText(character, (ConsoleColor)GetBackgroundColor(color), (ConsoleColor)GetForegroundColor(color), writeLine, x, y);
        }
        public static void DrawText(char character, ConsoleColor backgroundColor, ConsoleColor foregroundColor, int repeats, bool writeLine, int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            for (int i = 1; i <= repeats; i++)
            {
                Console.Write(character);
            }
            if (writeLine) Console.WriteLine();
        }
        public static void DrawText(char character, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool writeLine, int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(character);
            if (writeLine) Console.WriteLine();
        }
        #endregion
    }
}
