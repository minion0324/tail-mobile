using Xamarin.Forms;

namespace Tail.Controls.CustomVideoPlayer
{
    public class FileVideoSource : VideoSource
    {
        /// <summary>
        /// File property
        /// </summary>
        public static readonly BindableProperty FileProperty =
                  BindableProperty.Create(nameof(File), typeof(string), typeof(FileVideoSource));

        /// <summary>
        /// Get or set file property.
        /// </summary>
        public string File
        {
            set { SetValue(FileProperty, value); }
            get { return (string)GetValue(FileProperty); }
        }
    }
}
