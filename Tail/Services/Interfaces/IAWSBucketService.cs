using System;
using System.Threading.Tasks;

namespace Tail.Services.Interfaces
{
    public interface IAwsBucketService
    {
        event EventHandler OnProgress;
        Task<bool> UploadImageFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string BuketName, bool IsProfileImage);
        Task<bool> UploadByteArrayImageFileToAmazonBucketAsync(byte[] originalImagebyteArray, string FileName, string ThumbFileName, string BuketName, bool IsProfileImage);
        Task<bool> UploadVideoFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string ThumbnailPath, string BuketName);
        void DownlaodFile(string FilePath, string FileName, string BuketName);
        Task<bool> UploadLargeSizeVideoFromFileToAmazonBucketAsync(string FileName, string ThumbFileName, string FilePath, string ThumbnailPath, string BuketName);
    }
}
