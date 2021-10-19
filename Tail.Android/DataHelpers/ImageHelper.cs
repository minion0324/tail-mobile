using System;
using System.IO;
using Android.Graphics;
using Tail.Services.Interfaces;
using Tail.Droid.DataHelpers;
using Xamarin.Forms;
using Android.Media;
using Android.Provider;

[assembly: Dependency(typeof(ImageHelper))]
namespace Tail.Droid.DataHelpers
{
    public class ImageHelper : IImageHelper
    {

        public string GetLocalPath(string localFilename)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;
            //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
            var filePath =System.IO.Path.Combine(pictures, localFilename);
            if (System.IO.File.Exists(filePath))
            {
                return filePath.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public double GetImageSizeInMegabytes(string filePath)
        {
            FileInfo fInfo = new FileInfo(filePath);
            return (fInfo.Length / 1024f) / 1024f;
        }

        public byte[] GetBytesFromImage(string filePath, bool NeedCompression = true)
        {
            FileInfo fInfo = new FileInfo(filePath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            byte[] data = br.ReadBytes((int)numBytes);


            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(fInfo.FullName, options);
            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            data = ResizeImageAndroid(data, imageWidth, imageHeight);
            br.Close();


            return data;
        }
        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }

        public byte[] CompressImage(byte[] imageData, string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetThumbnailFromVideo(string path)
        {
            ImageResizeHelper imageResizeHelper = new ImageResizeHelper();
            
            Bitmap bitmap = ThumbnailUtils.CreateVideoThumbnail(path, ThumbnailKind.FullScreenKind);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                var thumbPath = imageResizeHelper.MaxResizeImageFromStream(bitmap, path, 180, 120);
                return thumbPath;
            }
            return null;
        }

        public string SaveShareImageToDirectory(byte[] imageData)
        {
            var imagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).ToString();
            imagePath = System.IO.Path.Combine(imagePath, "ShareImage");
            if (!System.IO.Directory.Exists(imagePath.ToString()))
            {
                Directory.CreateDirectory(imagePath);
            }
            imagePath = System.IO.Path.Combine(imagePath, "ImageToShare.jpg");
            bool doesExist = File.Exists(imagePath);
            if (doesExist)
                File.Delete(imagePath);

            System.IO.File.WriteAllBytes(imagePath, imageData);
            return imagePath;
        }
    }
}
