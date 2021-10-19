using System;
using System.IO;
using Tail.Services.Interfaces;
using Xamarin.Forms;
using System.Drawing;
using Tail.iOS.DataHelpers;
using System.Diagnostics;
using UIKit;
using CoreGraphics;
using AVFoundation;
using CoreMedia;
using Foundation;
using System.Linq;

[assembly: Dependency(typeof(ImageHelper))]
namespace Tail.iOS.DataHelpers
{
    public class ImageHelper : IImageHelper
    {
        public byte[] GetBytesFromImage(string filePath, bool NeedCompression = true)
        {
            FileInfo fInfo = new FileInfo(filePath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            byte[] dataOriginal = br.ReadBytes((int)numBytes);
            if (!NeedCompression)
            {
                br.Close();
                return dataOriginal;
            }
            UIImage image = UIImage.FromFile(filePath);
            double _width = (double)image.Size.Width;
            double _height = (double)image.Size.Height;
            byte[] data = ResizeImageIOS(dataOriginal, (float)_width, (float)_height);
            br.Close();
            return data;
        }
        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)height, 8,
                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                float copmressionValue = 0.9f;
                return resizedImage.AsJPEG(copmressionValue).ToArray();
            }
        }


        public byte[] CompressImage(byte[] imageData, string filePath)
        {
            float width; float height;
            double _width = 320; 
            double _height = 480;

            width = (float)_width;
            height = (float)_height;
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)height, 8,
                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);
                float copmressionValue = 0.2f;
                // save the image as a jpeg
                return resizedImage.AsJPEG(copmressionValue).ToArray();
            }
        }
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

        public double GetImageSizeInMegabytes(string filePath)
        {
            FileInfo fInfo = new FileInfo(filePath);
            return (fInfo.Length / 1024f) / 1024f;
        }
        public string GetLocalPath(string localFilename)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localPath = Path.Combine(documentsPath, localFilename);
                if (File.Exists(localPath))
                {
                    return localPath;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("From GetLocalPath " + ex.Message);
            }
            return string.Empty;
        }

        public string GetThumbnailFromVideo(string path)
        {
            ImageResizeHelper imageResizeHelper = new ImageResizeHelper();
            AVAssetImageGenerator imageGenerator = new AVAssetImageGenerator(AVAsset.FromUrl(NSUrl.FromFilename(path)));
            imageGenerator.AppliesPreferredTrackTransform = true;
            CMTime actualTime;
            NSError error;
            CGImage cgImage = imageGenerator.CopyCGImageAtTime(new CMTime(1, 1), out actualTime, out error);
            var img = new UIImage(cgImage);
            Uri url = new Uri(path);
            string fileName = url.Segments.Last();
            var thumbPath = imageResizeHelper.MaxResizeImageFromStream(img.AsJPEG().AsStream(), fileName, 180, 120);
            return thumbPath;
           
        }
        public string SaveShareImageToDirectory(byte[] imageData)
        {
            var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            imagePath = Path.Combine(imagePath, "ShareImage");
            if (!System.IO.Directory.Exists(imagePath.ToString()))
            {
                Directory.CreateDirectory(imagePath);
            }
            imagePath = Path.Combine(imagePath, "ImageToShare.jpg");
            bool doesExist = File.Exists(imagePath);
            if (doesExist)
                File.Delete(imagePath);
            NSData data = NSData.FromArray(imageData);
            NSError err = null;
            data.Save(imagePath, false, out err);
            return imagePath;
        }
    }
}
