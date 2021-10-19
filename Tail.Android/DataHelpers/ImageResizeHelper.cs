using System;
using System.IO;
using Android.Graphics;
using Android.Media;
using Tail.Droid.DataHelpers;
using Tail.Models;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResizeHelper))]
namespace Tail.Droid.DataHelpers
{
    public class ImageResizeHelper : IImageResizeHelper
    {
        const string TemporalDirectoryName = "TmpMedia";
        public string MaxResizeImage(string sourceImagePath, float maxWidth, float maxHeight)
        {
            byte[] imageBytes;

            var originalImage = BitmapFactory.DecodeFile(sourceImagePath);
            var rotation = GetRotation(sourceImagePath);
            var width = (maxWidth);
            var height = (maxHeight);
            var scaledImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, true);

            Bitmap rotatedImage = scaledImage;
            if (rotation != 0)
            {
                var matrix = new Matrix();
                matrix.PostRotate(rotation);
                rotatedImage = Bitmap.CreateBitmap(scaledImage, 0, 0, scaledImage.Width, scaledImage.Height, matrix, true);
                scaledImage.Recycle();
                scaledImage.Dispose();
            }

            using (var ms = new MemoryStream())
            {
                rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, 90, ms);
                imageBytes = ms.ToArray();
            }

            
            originalImage?.Dispose();
            rotatedImage?.Dispose();
           var thumbPath = SaveImageToDirectory(imageBytes, sourceImagePath);
            return thumbPath;
        }
        private string SaveImageToDirectory(byte[] imageBytes, string path)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            var ext = System.IO.Path.GetExtension(path) ?? string.Empty;
            var thumbnailImagePath = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileName}-THUMBNAIL{ext}");
            File.WriteAllBytes(thumbnailImagePath, imageBytes);
            return thumbnailImagePath;
        }
        static int GetRotation(string filePath)
        {
            using (var ei = new ExifInterface(filePath))
            {
                var orientation = (Android.Media.Orientation)ei.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);

                switch (orientation)
                {
                    case Android.Media.Orientation.Rotate90:
                        return 90;
                    case Android.Media.Orientation.Rotate180:
                        return 180;
                    case Android.Media.Orientation.Rotate270:
                        return 270;
                    default:
                        return 0;
                }
            }
        }

        

        public string MaxResizeImageFromStream(Bitmap sourceImageData, string fileName, float maxWidth, float maxHeight)
        {
            byte[] imageBytes;

            var originalImage = sourceImageData;
            var fileNamewithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
            var thumbnailImagePath = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileNamewithoutExtension}-THUMBNAIL.jpg");
            
            var rotation = GetRotation(fileName);
            var width = (maxWidth);
            var height = (maxHeight);
            var scaledImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, true);

            Bitmap rotatedImage = scaledImage;
            if (rotation != 0)
            {
                var matrix = new Matrix();
                matrix.PostRotate(rotation);
                rotatedImage = Bitmap.CreateBitmap(scaledImage, 0, 0, scaledImage.Width, scaledImage.Height, matrix, true);
                scaledImage.Recycle();
                scaledImage.Dispose();
            }

            using (var ms = new MemoryStream())
            {
                rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, 90, ms);
                imageBytes = ms.ToArray();
            }


            originalImage?.Dispose();
            rotatedImage?.Dispose();
            var thumbPath = SaveImageToDirectory(imageBytes, thumbnailImagePath);
            return thumbPath;
        }

        public string MaxResizeImageFromStream(System.IO.Stream sourceImageData, string fileName, float maxWidth, float maxHeight)
        {
            throw new NotImplementedException();
        }
    }
}
