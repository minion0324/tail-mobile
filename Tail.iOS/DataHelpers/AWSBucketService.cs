using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Transfer;
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.iOS.DataHelpers;
using UIKit;
using Xamarin.Forms;
using Foundation;
using AVFoundation;

[assembly: Dependency(typeof(AwsBucketService))]
namespace Tail.iOS.DataHelpers
{
    public class AwsBucketService : IAwsBucketService
    {
        public  event EventHandler OnProgress;
        public async Task<bool> UploadImageFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string BuketName, bool IsProfileImage)
        {
            bool hasSuccessResponse;
            try
            {
                //Create Thumb Image
                byte[] originalImagebyteArray = File.ReadAllBytes(FilePath);
                var thumbImagebyteArray = GetThumbnailFromByteArray(originalImagebyteArray, IsProfileImage);
                string thumbImageFilePath;
                thumbImageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);

                // Initialize the Amazon Cognito credentials provider
                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    Constants.AWSCredential, // Identity pool ID
                    RegionEndpoint.USEast2 // Region
                );

                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);
                var transferUtility = new TransferUtility(s3Client);

                var transferUploadRequest = new TransferUtilityUploadRequest();
                transferUploadRequest.CannedACL = S3CannedACL.PublicRead;
                transferUploadRequest.DisableMD5Stream = true;
                transferUploadRequest.FilePath = FilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;
               

         
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


        public void uploadRequest_UploadPartProgressEvent(object sender, UploadProgressArgs e)
        {
            // Process event.
            Console.WriteLine("{0}/{1}", e.TransferredBytes, e.TotalBytes);
            decimal _percentage = Decimal.Divide(e.TransferredBytes, e.TotalBytes)*100;
            Console.WriteLine("Progress : "+ Math.Round(_percentage, 2) + " %");
            OnProgress?.Invoke(sender, e);
        }

        public async Task<bool> UploadByteArrayImageFileToAmazonBucketAsync(byte[] originalImagebyteArray, string FileName, string ThumbFileName, string BuketName, bool IsProfileImage)
        {

            bool hasSuccessResponse;
            try
            {
                //Create Thumb Image
                var thumbImagebyteArray = GetThumbnailFromByteArray(originalImagebyteArray, IsProfileImage);
                string thumbImageFilePath;
                thumbImageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp3.jpeg");
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);

                //Create Orginal Image
                string originalImageFilePath;
                originalImageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp2.jpeg");
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

        public async Task<bool> UploadVideoFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string ThumbnailPath, string BuketName)
        {
            bool hasSuccessResponse;
            try
            {



                var thumbImagebyteArray = GetVideoThumbnail(FilePath);
                string thumbImageFilePath;
                thumbImageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
                File.WriteAllBytes(thumbImageFilePath, thumbImagebyteArray);

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
                transferUploadRequest.FilePath = FilePath;
                transferUploadRequest.BucketName = BuketName;
                transferUploadRequest.Key = FileName;


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



                var thumbImagebyteArray = GetVideoThumbnail(FilePath);
                string thumbImageFilePath;
                thumbImageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ThumbFileName);
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
                Debug.WriteLine("Exception message - " + ex.Message);
            }
        }
        private byte[] GetThumbnailFromByteArray(byte[] originalImagebyteArray, bool IsProfileImage)
        {
            try
            {
                float width = ((float)UIScreen.MainScreen.Bounds.Width-20)*2;
                float height = Constants.PostThumbImageHeight;
                if (IsProfileImage)
                {
                    width = Constants.UserThumbImageWidth;
                    height = Constants.UserThumbImageHeight;
                }

                UIImage originalImage = ImageHelper.ImageFromByteArray(originalImagebyteArray);

                var originalHeight = originalImage.Size.Height;
                var originalWidth = originalImage.Size.Width;

                nfloat newHeight = 0;
                nfloat newWidth = 0;

                if (originalHeight > originalWidth)
                {
                    newHeight = height;
                    nfloat ratio = originalHeight / height;
                    newWidth = originalWidth / ratio;
                }
                else
                {
                    newWidth = width;
                    nfloat ratio = originalWidth / width;
                    newHeight = originalHeight / ratio;
                }

                width = (float)newWidth;
                height = (float)newHeight;

                UIGraphics.BeginImageContext(new SizeF(width, height));
                originalImage.Draw(new RectangleF(0, 0, width, height));
                var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                var thumbImagebyteArray = resizedImage.AsJPEG().ToArray();
                resizedImage.Dispose();

                return thumbImagebyteArray;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetThumbnailFromByteArray: Exception message - " + ex.Message);
                return new byte[0];
            }
        }
        private byte[] GetVideoThumbnail(string videoPath)
        {
            float width = ((float)UIScreen.MainScreen.Bounds.Width - 20) * 2;
            float height = Constants.PostThumbImageHeight;

            CoreMedia.CMTime actualTime;
            NSError outError;
            using (var asset = AVAsset.FromUrl(NSUrl.FromFilename(videoPath)))
            using (var imageGen = new AVAssetImageGenerator(asset))
            {
                imageGen.AppliesPreferredTrackTransform = true;
                using (var imageRef = imageGen.CopyCGImageAtTime(new CoreMedia.CMTime(1, 1), out actualTime, out outError))
                {

                    if (imageRef == null)
                        return new byte[0];
                    var originalImage = UIImage.FromImage(imageRef);
                    var originalHeight = originalImage.Size.Height;
                    var originalWidth = originalImage.Size.Width;

                    nfloat newHeight = 0;
                    nfloat newWidth = 0;

                    if (originalHeight > originalWidth)
                    {
                        newHeight = height;
                        nfloat ratio = originalHeight / height;
                        newWidth = originalWidth / ratio;
                    }
                    else
                    {
                        newWidth = width;
                        nfloat ratio = originalWidth / width;
                        newHeight = originalHeight / ratio;
                    }

                    width = (float)newWidth;
                    height = (float)newHeight;

                    UIGraphics.BeginImageContext(new SizeF(width, height));
                    originalImage.Draw(new RectangleF(0, 0, width, height));
                    var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();

                    var thumbImagebyteArray = resizedImage.AsJPEG().ToArray();
                    resizedImage.Dispose();

                    return thumbImagebyteArray;
                }
            }
            

        }
    }
}