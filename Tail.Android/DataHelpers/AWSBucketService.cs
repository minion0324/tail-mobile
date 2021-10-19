using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Transfer;
using Android.Graphics;
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.Droid.DataHelpers;
using Xamarin.Forms;
using Android.Media;
using Android.Provider;
using Tail.Services.ApplicationServices;

[assembly: Dependency(typeof(AwsBucketService))]
namespace Tail.Droid.DataHelpers
{
    public class AwsBucketService : IAwsBucketService
    {
        public event EventHandler OnProgress;
        public async Task<bool> UploadImageFromFileToAmazonBucketAsync( string FileName, string ThumbFileName, string FilePath, string BuketName, bool IsProfileImage)
        {
            bool hasSuccessResponse;
            try
            {
                byte[] originalImagebyteArray = File.ReadAllBytes(FilePath);
                var thumbImagebyteArray = GetThumbnailFromByteArray(originalImagebyteArray, IsProfileImage);
                string thumbImageFilePath;
                thumbImageFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);

           
                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential, 
                    RegionEndpoint.USEast2 
                );
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

                var transferUtility = new TransferUtility(s3Client);

                //uploading original image
                var transferUploadRequest = new TransferUtilityUploadRequest();
                transferUploadRequest.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequest.FilePath = FilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;
                //transferUploadRequest.PartSize = 500000;

                transferUploadRequest.UploadProgressEvent +=
                    new EventHandler<UploadProgressArgs>
                        (uploadRequest_UploadPartProgressEvent);

                await transferUtility.UploadAsync(transferUploadRequest);

                //uploading thumb image
                var transferUploadRequestThumb = new TransferUtilityUploadRequest();
                transferUploadRequestThumb.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequestThumb.FilePath = thumbImageFilePath;
                transferUploadRequestThumb.BucketName = BuketName;
                transferUploadRequestThumb.Key = ThumbFileName;
                await transferUtility.UploadAsync(transferUploadRequestThumb);

                hasSuccessResponse = true;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                hasSuccessResponse = false;
            }
            return hasSuccessResponse;
        }


        public void uploadRequest_UploadPartProgressEvent(object sender, UploadProgressArgs e)
        {
            Console.WriteLine("{0}/{1}", e.TransferredBytes, e.TotalBytes);
            decimal _percentage = Decimal.Divide(e.TransferredBytes, e.TotalBytes) * 100;
           
            Console.WriteLine("Progress : " + Math.Round(_percentage, 2) + " %");
            OnProgress?.Invoke(sender, e);
        }

        public async Task<bool> UploadByteArrayImageFileToAmazonBucketAsync(byte[] originalImagebyteArray, string FileName, string ThumbFileName, string BuketName, bool IsProfileImage)
        {
            bool hasSuccessResponse;
            try
            {
                
                var thumbImagebyteArray = GetThumbnailFromByteArray(originalImagebyteArray,IsProfileImage);
                string thumbImageFilePath;
                thumbImageFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp3.jpeg");
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);

                string originalImageFilePath;
                originalImageFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp2.jpeg");
                File.WriteAllBytes(originalImageFilePath, originalImagebyteArray);



                // Initialize the Amazon Cognito credentials provider
                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential, // Identity pool ID
                    RegionEndpoint.USEast2 // Region
                );

                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

                var transferUtility = new TransferUtility(s3Client);

                //uploading original image
                var transferUploadRequest = new TransferUtilityUploadRequest();
                transferUploadRequest.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequest.FilePath = originalImageFilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;
               // transferUploadRequest.PartSize = 500000;

                transferUploadRequest.UploadProgressEvent +=
                    new EventHandler<UploadProgressArgs>
                        (uploadRequest_UploadPartProgressEvent);
                await transferUtility.UploadAsync(transferUploadRequest);

                //uploading thumb image
                var transferUploadRequestThumb = new TransferUtilityUploadRequest();
                transferUploadRequestThumb.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequestThumb.FilePath = thumbImageFilePath;
                transferUploadRequestThumb.BucketName = BuketName;
                transferUploadRequestThumb.Key = ThumbFileName;
                await transferUtility.UploadAsync(transferUploadRequestThumb);

                hasSuccessResponse = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UploadImageFileToAmazonBucketAsync: Exception message - " + ex.Message);
                hasSuccessResponse = false;
            }
            return hasSuccessResponse;
        }
        private byte[]GetVideoThumbnailFromPath(string videoPath)
        {
            float width = Constants.PostThumbImageWidth;
            if (CommonSingletonUtility.SharedInstance.DeviceWidth !=0)
                 width = (CommonSingletonUtility.SharedInstance.DeviceWidth-20)*2;
            float height = Constants.PostThumbImageHeight;
            var originalImage = ThumbnailUtils.CreateVideoThumbnail(videoPath, ThumbnailKind.FullScreenKind);
            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;
            float newHeight = 0;
            float newWidth = 0;
            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);

            originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);

                resizedImage.Recycle();

                return ms.ToArray();
            }
        }
        private byte[] GetThumbnailFromByteArray(byte[] originalImagebyteArray, bool IsProfileImage)
        {
            try
            {
                float width = Constants.PostThumbImageWidth;
                if (CommonSingletonUtility.SharedInstance.DeviceWidth != 0)
                    width = (CommonSingletonUtility.SharedInstance.DeviceWidth - 20) * 2;
                float height = Constants.PostThumbImageHeight;
                if (IsProfileImage)
                {
                    width = Constants.UserThumbImageWidth;
                    height = Constants.UserThumbImageHeight;
                }
                // Load the bitmap 
                BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
                options.InPurgeable = true; // inPurgeable is used to free up memory while required
                Bitmap originalImage = BitmapFactory.DecodeByteArray(originalImagebyteArray, 0, originalImagebyteArray.Length, options);

                float newHeight = 0;
                float newWidth = 0;

                var originalHeight = originalImage.Height;
                var originalWidth = originalImage.Width;

                if (originalHeight > originalWidth)
                {
                    newHeight = height;
                    float ratio = originalHeight / height;
                    newWidth = originalWidth / ratio;
                }
                else
                {
                    newWidth = width;
                    float ratio = originalWidth / width;
                    newHeight = originalHeight / ratio;
                }

                Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);

                originalImage.Recycle();

                using (MemoryStream ms = new MemoryStream())
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);

                    resizedImage.Recycle();

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetThumbnailFromByteArray: Exception message - " + ex.Message);
                return new byte[0];
            }
        }

        public async Task<bool> UploadVideoFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string ThumbnailPath, string BuketName)
        {
            bool hasSuccessResponse;
            try
            {
              
                var thumbImagebyteArray = GetVideoThumbnailFromPath(FilePath);
                string thumbImageFilePath;
                thumbImageFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);


                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential,
                    RegionEndpoint.USEast2
                );
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

                var transferUtility = new TransferUtility(s3Client);

                //uploading original image
                var transferUploadRequest = new TransferUtilityUploadRequest();
                transferUploadRequest.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequest.FilePath = FilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;
               // transferUploadRequest.PartSize = 1 * 1024 * 1024;

                transferUploadRequest.UploadProgressEvent +=
                    new EventHandler<UploadProgressArgs>
                        (uploadRequest_UploadPartProgressEvent);
                await transferUtility.UploadAsync(transferUploadRequest);

                //uploading thumb image
                var transferUploadRequestThumb = new TransferUtilityUploadRequest();
                transferUploadRequestThumb.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequestThumb.FilePath = thumbImageFilePath;
                transferUploadRequestThumb.BucketName = BuketName;
                transferUploadRequestThumb.Key = ThumbFileName;
                await transferUtility.UploadAsync(transferUploadRequestThumb);

                hasSuccessResponse = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("UploadFileToAmazonBucketAsync: Exception message - " + ex.Message);
                hasSuccessResponse = false;
            }
            return hasSuccessResponse;
        }
        public async Task<bool> UploadLargeSizeVideoFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string ThumbnailPath, string BuketName)
        {
            bool hasSuccessResponse;
            try
            {


                //Create Thumb Image
                var thumbImagebyteArray = GetVideoThumbnailFromPath(FilePath);
                string thumbImageFilePath;
                thumbImageFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);


                // Initialize the Amazon Cognito credentials provider
                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential, // Identity pool ID
                    RegionEndpoint.USEast2 // Region
                );

                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

                var config = new TransferUtilityConfig();
                config.ConcurrentServiceRequests = 10;
                config.MinSizeBeforePartUpload = 10 * 1024 * 1024;

                var transferUtility = new TransferUtility(s3Client);

                //uploading original image
                var transferUploadRequest = new TransferUtilityUploadRequest();
                transferUploadRequest.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequest.FilePath = FilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;
                transferUploadRequest.PartSize = 10 * 1024 * 1024;

                transferUploadRequest.UploadProgressEvent +=
                    new EventHandler<UploadProgressArgs>
                        (uploadRequest_UploadPartProgressEvent);
                await transferUtility.UploadAsync(transferUploadRequest);

                //uploading thumb image
                var transferUploadRequestThumb = new TransferUtilityUploadRequest();
                transferUploadRequestThumb.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequestThumb.FilePath = thumbImageFilePath;
                transferUploadRequestThumb.BucketName = BuketName;
                transferUploadRequestThumb.Key = ThumbFileName;
                await transferUtility.UploadAsync(transferUploadRequestThumb);

                hasSuccessResponse = true;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("UploadFileToAmazonBucketAsync: Exception message - " + ex.Message);
                hasSuccessResponse = false;
            }
            return hasSuccessResponse;

        }
        public void DownlaodFile(string FilePath, string FileName, string BuketName)
        {

            try
            {
                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential, // Identity pool ID
                    RegionEndpoint.USEast2 // Region
                );

                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);
                var transferUtility = new TransferUtility(s3Client);
                transferUtility.DownloadAsync(FilePath, BuketName, FileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
               
            }
        }
    }
}

