

namespace Tail.Services.Interfaces
{
    public interface IImageHelper
    {
        byte[] GetBytesFromImage(string filePath, bool NeedCompression = true);
        double GetImageSizeInMegabytes(string filePath);
        string GetLocalPath(string localFilename);
        byte[] CompressImage(byte[] imageData, string filePath);
        string GetThumbnailFromVideo(string path);
        string SaveShareImageToDirectory(byte[] imageData);
    }
}
