using System;

namespace Tail.Controls.CustomVideoPlayer
{
    /// <summary>
    /// Video player status.
    /// </summary>
    public interface IVideoPlayerController
    {
        /// <summary>
        /// Video status like playing paused.
        /// </summary>
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
