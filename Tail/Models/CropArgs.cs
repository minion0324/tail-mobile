using System;
namespace Tail.Models
{
    public class CropArgs
    {
        public byte[] ImageAsByte { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Action CropRefreshCallback { get; set; }

    }
}
