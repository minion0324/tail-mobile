
using System.IO;

namespace Tail.Services.Interfaces
{
    public interface IImageResizeHelper
    {
        string MaxResizeImage(string sourceImagePath, float maxWidth, float maxHeight);
        string MaxResizeImageFromStream(Stream sourceImageData, string fileName, float maxWidth, float maxHeight);
    }
}
