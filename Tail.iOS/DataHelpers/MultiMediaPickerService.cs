using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using GMImagePicker;
using Photos;
using Tail.Models;
using Tail.Services.Interfaces;
using UIKit;
using Tail.Services.Helper;

namespace Tail.iOS.DataHelpers
{
    public class MultiMediaPickerService : IMultiMediaPickerService
    {
        const string TemporalDirectoryName = "TmpMedia";

        //Events
        public event EventHandler<MediaFile> OnMediaPicked;
        public event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;
        public event EventHandler OnCancelled;
        GMImagePickerController currentPicker;
        TaskCompletionSource<IList<MediaFile>> mediaPickTcs;

        public void Clean()
        {
            var documentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), TemporalDirectoryName);

            if (Directory.Exists(documentsDirectory))
            {
                Directory.Delete(documentsDirectory);
            }
        }

        public async Task<IList<MediaFile>> PickPhotosAsync()
        {
            return await PickMediaAsync("Select Images", PHAssetMediaType.Image);
        }

        public async Task<IList<MediaFile>> PickVideosAsync()
        {
            return await PickMediaAsync("Select Videos", PHAssetMediaType.Video);
        }

        async Task<IList<MediaFile>> PickMediaAsync(string title, PHAssetMediaType type)
        {

            mediaPickTcs = new TaskCompletionSource<IList<MediaFile>>();
            currentPicker = new GMImagePickerController()
            {
                Title = title,
                MediaTypes = new[] { type }
            };
            currentPicker.ShouldSelectAsset += (sender, args) => args.Cancel = currentPicker.SelectedAssets.Count == 10;
            currentPicker.FinishedPickingAssets += FinishedPickingAssets;
            currentPicker.Canceled += Picker_Canceled;
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            await vc.PresentViewControllerAsync(currentPicker, true);

            var results = await mediaPickTcs.Task;

            currentPicker.FinishedPickingAssets -= FinishedPickingAssets;
            OnMediaPickedCompleted?.Invoke(this, results);
            return results;
        }

        private void Picker_Canceled(object sender, EventArgs e)
        {
            OnCancelled.Invoke(this, e);
        }

        async void FinishedPickingAssets(object sender, MultiAssetEventArgs args)
        {
            IList<MediaFile> results = new List<MediaFile>();
            TaskCompletionSource<IList<MediaFile>> tcs = new TaskCompletionSource<IList<MediaFile>>();

            var options = new PHImageRequestOptions()
            {
                NetworkAccessAllowed = true
            };

            options.Synchronous = false;
            options.ResizeMode = PHImageRequestOptionsResizeMode.Fast;
            options.DeliveryMode = PHImageRequestOptionsDeliveryMode.HighQualityFormat;
            bool completed = false;
            for (var i = 0; i < args.Assets.Length; i++)
            {
                var asset = args.Assets[i];

                string fileName = string.Empty;
                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    fileName = PHAssetResource.GetAssetResources(asset).FirstOrDefault().OriginalFilename;

                }

                switch (asset.MediaType)
                {
                    case PHAssetMediaType.Video:

                        AddVideo(asset, options, fileName, results, args, tcs, completed);
                        break;
                    default:

                        AddImage(asset, options, fileName, results, args, tcs, completed);
                        break;
                }
            }


            mediaPickTcs?.TrySetResult(await tcs.Task);
        }
        private void AddVideo(PHAsset asset, PHImageRequestOptions options, string fileName, IList<MediaFile> results, MultiAssetEventArgs args, TaskCompletionSource<IList<MediaFile>> tcs, bool completed)
        {
            PHImageManager.DefaultManager.RequestImageForAsset(asset, new SizeF(150.0f, 150.0f),
                           PHImageContentMode.AspectFill, options, async (img, info) =>
                           {
                               var startIndex = fileName.IndexOf(".", StringComparison.CurrentCulture);

                               string path = "";
                               if (startIndex != -1)
                               {
                                   path = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileName.Substring(0, startIndex)}-THUMBNAIL.JPG");
                               }
                               else
                               {
                                   path = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, string.Empty);

                               }

                               if (!File.Exists(path))
                               {

                                   img.AsJPEG().Save(path, true);
                               }

                               TaskCompletionSource<string> tvcs = new TaskCompletionSource<string>();

                               var vOptions = new PHVideoRequestOptions();
                               vOptions.NetworkAccessAllowed = true;
                               vOptions.Version = PHVideoRequestOptionsVersion.Original;
                               vOptions.DeliveryMode = PHVideoRequestOptionsDeliveryMode.FastFormat;


                               PHImageManager.DefaultManager.RequestAvAsset(asset, vOptions, (avAsset, audioMix, vInfo) =>
                               {
                                   var vPath = FileHelper.GetOutputPath(MediaFileType.Video, TemporalDirectoryName, fileName);

                                   if (!File.Exists(vPath))
                                   {
                                       AVAssetExportSession exportSession = new AVAssetExportSession(avAsset, AVAssetExportSession.PresetHighestQuality);

                                       exportSession.OutputUrl = NSUrl.FromFilename(vPath);
                                       exportSession.OutputFileType = AVFileType.QuickTimeMovie;


                                       exportSession.ExportAsynchronously(() =>
                                       {
                                           Console.WriteLine(exportSession.Status);

                                           tvcs.TrySetResult(vPath);

                                       });

                                   }

                               });

                               var videoUrl = await tvcs.Task;
                               var meFile = new MediaFile()
                               {
                                   Type = MediaFileType.Video,
                                   Path = videoUrl,
                                   PreviewPath = path
                               };
                               results.Add(meFile);
                               OnMediaPicked?.Invoke(this, meFile);

                               if (args.Assets.Length == results.Count && !completed)
                               {
                                   completed = true;
                                   tcs.TrySetResult(results);
                               }

                           });
        }
        private void AddImage(PHAsset asset, PHImageRequestOptions options, string fileName, IList<MediaFile> results, MultiAssetEventArgs args, TaskCompletionSource<IList<MediaFile>> tcs, bool completed)
        {
            PHImageManager.DefaultManager.RequestImageData(asset, options, (data, dataUti, orientation, info) =>
            {

                string path = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, fileName);

                if (!File.Exists(path))
                {
                    Debug.WriteLine(dataUti);
                    var imageData = data;
                    var image = UIImage.LoadFromData(imageData);
                    imageData = image.AsJPEG((nfloat)0.6);
                    imageData?.Save(path, true);


                }
                var thumbPath = MaxResizeImage(path, fileName, 180, 120);
                var meFile = new MediaFile()
                {
                    Type = MediaFileType.Image,
                    Path = path,
                    PreviewPath = thumbPath,

                };

                results.Add(meFile);
                OnMediaPicked?.Invoke(this, meFile);
                if (args.Assets.Length == results.Count && !completed)
                {
                    completed = true;
                    tcs.TrySetResult(results);
                }

            });
        }
        public string MaxResizeImage(string sourceImagePath, string fileName, float maxWidth, float maxHeight)
        {
            var sourceImage = UIImage.FromFile(sourceImagePath);
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImagePath;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            var thumbPath = SaveImageToDirectory(resultImage, fileName);
            return thumbPath;
        }
        private string SaveImageToDirectory(UIImage image, string filename)
        {
            var documentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var thumbPath = Path.Combine(documentDirectoryPath, "TailThumbnails");
            if (!Directory.Exists(thumbPath))
            {
                Directory.CreateDirectory(thumbPath);
            }
            var imageName = Path.Combine(thumbPath, filename);
            NSData imgData = image.AsJPEG();
            NSError err = null;
            if (imgData.Save(imageName, false, out err))
            {
                Console.WriteLine("saved as " + imageName);
            }
            else
            {
                Console.WriteLine("NOT saved as " + imageName + " because" + err.LocalizedDescription);
            }
            return imageName;
        }
    }
}
