using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace Compression
{
    class Program
    {
        static void Main(string[] args)
        {
            //1L = Lowest Quality
            //25L = Low Quality
            //50L = Medium Quality
            //75L = High Quality
            //100L = Highest Quality

            GetCompressedImage(@"D:\test", @"D:\test\compressed", 1);
        }

        private static void GetCompressedImage(string filePath, string destinationPath, int imageQuality)
        {

            string[] files = Directory.GetFiles(filePath);

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToUpper();
                if (ext == ".PNG" || ext == ".JPG" || ext == ".svc")
                    CompressImage(file, destinationPath, imageQuality);
                else
                    CompressOtherThanImage(file, destinationPath);
            }

            Console.WriteLine("Compressed Images has been stored to\n" + destinationPath);
            
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static void CompressImage(string SoucePath, string DestPath, int compressionLevel)
        {
            var FileName = Path.GetFileName(SoucePath);
            DestPath = DestPath + "\\" + FileName;

            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, compressionLevel);

                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);

            }
        }
        public static void CompressOtherThanImage(string SoucePath, string compressedFileName)
        {
            var FileName = Path.GetFileName(SoucePath);
            compressedFileName = compressedFileName + "\\" + FileName;

            using (FileStream sourceFile = File.OpenRead(SoucePath))
            using (FileStream destinationFile = File.Create(compressedFileName))
            using (GZipStream output = new GZipStream(destinationFile, CompressionMode.Compress))
            {
                sourceFile.CopyTo(output);
            }
        }
    }
}
