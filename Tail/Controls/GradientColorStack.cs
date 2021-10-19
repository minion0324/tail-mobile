using Xamarin.Forms;
using Tail.Common;
namespace Tail.Controls
{
    public class GradientColorStack : Frame
    {
        public Color StartColor { get; set; }

        public Color EndColor { get; set; }

        public int ButtonRadius { get; set; }

        public bool IsShadowVisible { get; set; }

        public Color ShadowColor { get; set; }

        public GradientDirection GradientDirection { get; set; }
    }
}
