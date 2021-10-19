using System;
using Xamarin.Forms;

namespace Tail.Controls
{
    public class CustomStackLayout : StackLayout
    {
        public delegate void DrawImageHandler(Action<byte[]> action);
        public DrawImageHandler OnDrawing { get; set; }
       
    }
}
