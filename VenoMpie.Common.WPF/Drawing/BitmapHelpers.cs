using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VenoMpie.Utils.Common.Drawing
{
    public static class BitmapHelpers
    {
        public static BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap, PixelFormat format)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, format, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
        public static BitmapImage ConvertStreamToBitmapImage(Stream source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        public static BitmapSource GetFileIcon(string filePath, int width, int height)
        {
            Bitmap outbm = new Bitmap(width, height);
            var graph = Graphics.FromImage(outbm);
            graph.DrawImage(Icon.ExtractAssociatedIcon(filePath).ToBitmap(), new Rectangle(0, 0, width, height));
            return ConvertBitmapToBitmapSource(outbm, PixelFormats.Bgra32);
        }
    }
}
