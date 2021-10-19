using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tail.Models;

namespace Tail.Services.Interfaces
{
    public interface IMultiMediaPickerService
    {
        event EventHandler<MediaFile> OnMediaPicked;
        event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;
        event EventHandler OnCancelled;
        Task<IList<MediaFile>> PickPhotosAsync();
        Task<IList<MediaFile>> PickVideosAsync();
        void Clean();
    }
}
