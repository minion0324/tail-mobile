using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Foundation;
using Tail.iOS.DataHelpers;
using Tail.Services.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResizeHelper))]
namespace Tail.iOS.DataHelpers
{
    public class ImageResizeHelper: IImageResizeHelper
    {
        public string MaxResizeImage(string sourceImagePath, float maxWidth, float maxHeight)
        {
            var sourceImage = UIImage.FromFile(sourceImagePath);
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImagePath;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            Uri url = new Uri(sourceImagePath);
            string fileName = url.Segments.Last();
            var thumbPath = SaveImageToDirectory(resultImage, fileName);
            return thumbPath;
        }
        public string MaxResizeImageFromStream(Stream sourceImageData, string fileName, float maxWidth, float maxHeight)
        {
            var imageData = NSData.FromStream(sourceImageData);
            var sourceImage = UIImage.LoadFromData(imageData);
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1)
            {
                var thumbnailPath = SaveImageToDirectory(sourceImage, fileName);
                return thumbnailPath;
            }
            
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
           
            var thumbPath = SaveImageToDirectory(resultImage, fileName);
            return thumbPath;
        }
        private string SaveImageToDirectory(UIImage image, string filename)
        {
            var documentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var thumbPath = Path.Combine(documentDirectoryPath, "TailThumbnails");
            if (!System.IO.Directory.Exists(thumbPath.ToString()))
            {
                Directory.CreateDirectory(thumbPath);
            }
            var imageName = Path.Combine(thumbPath, filename);
            NSData imgData = image.AsJPEG();
            NSError err = null;
            if (imgData.Save(imageName, false, out err))
            {
                Console.WriteLine("saved as " + imageName);
            }
            else
            {
                Console.WriteLine("NOT saved as " + imageName + " because" + err.LocalizedDescription);
            }
            return imageName;
        }
    }
}
