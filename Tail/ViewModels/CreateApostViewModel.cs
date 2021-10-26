using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class CreateApostViewModel : PageViewModelBase
    {
        Command _postNow;
        Command _postAPickCommand;
        Command _imageTapCommand;
        Command _discardCommand;
        ObservableCollection<MediaFile> _media;
        string _postContent;
        string _userImage;
        string _userName;
        int _currentProgressCount = 1;
        decimal _progressPercentage = 0;
        string _progressCountDisplay;
        string _progressPercentageDisplay;
        bool _isNormalIndicator;
        public ObservableCollection<MediaFile> Media
        {
            get => _media;
            set => SetProperty(ref _media, value);
        }
        int _imageSelectedCount;
        public int ImageSelectedCount
        {
            get => _imageSelectedCount;
            set => SetProperty(ref _imageSelectedCount, value);
        }
        public string PostContent
        {
            get => _postContent;
            set => SetProperty(ref _postContent, value);
        }

        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        public int CurrentProgressCount
        {
            get => _currentProgressCount;
            set => SetProperty(ref _currentProgressCount, value);
        }
        public decimal ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }
        public string ProgressCountDisplay
        {
            get => _progressCountDisplay;
            set => SetProperty(ref _progressCountDisplay, value);
        }
        public string ProgressPercentageDisplay
        {
            get => _progressPercentageDisplay;
            set => SetProperty(ref _progressPercentageDisplay, value);
        }
        public bool IsNormalIndicator
        {
            get => _isNormalIndicator;
            set => SetProperty(ref _isNormalIndicator, value);
        }
        public Command PostNow => _postNow ?? (_postNow = new Command(async () => await Handle_PostCommand()));
        public Command PostAPickCommand => _postAPickCommand ?? (_postAPickCommand = new Command(async () => await Handle_PostAPickCommand()));
        public Command ImageTapCommand => _imageTapCommand ?? (_imageTapCommand = new Command(async (item) => await Handle_ImageTapCommandCommandAsync(item)));
        public Command DiscardCommand => _discardCommand ?? (_discardCommand = new Command(() => Handle_DiscardCommand()));

        private void Handle_DiscardCommand()
        {
            Back.Execute(null);

        }

        private async Task Handle_ImageTapCommandCommandAsync(object item)
        {
            var tappedItem = item as MediaFile;
            if (tappedItem.Type == MediaFileType.Image)
                await PopupNavigation.Instance.PushAsync(new PostImagePopup(tappedItem.Path));
            else if (tappedItem.Type == MediaFileType.Video)
                await PopupNavigation.Instance.PushAsync(new PostVideoPopup(tappedItem.Path));

        }

        public CreateApostViewModel()
        {
            Media = new ObservableCollection<MediaFile>();
            if (!string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.UserImage) && SettingsService.Instance.LoggedUserDetails.UserImage != Constants.DEFAULT_USERIMAGE)
            {
                string _fullurl = TailUtils.GetThumbProfileImage(SettingsService.Instance.LoggedUserDetails.UserImage);
                UserImage = _fullurl;
            }
            else
            {
                UserImage = Constants.DEFAULT_USERIMAGE;
            }

            DependencyService.Get<IAwsBucketService>().OnProgress += (s, e) =>
            {
                UploadProgressArgs evArgs = (UploadProgressArgs)e;
                TransferUtilityUploadRequest uploadObj = (TransferUtilityUploadRequest)s;
                var _existingItem = Media.FirstOrDefault(p => p.Path == uploadObj.FilePath);
                if (_existingItem != null)
                {
                    int indexValue = Media.IndexOf(_existingItem);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Media[indexValue].Progress = Math.Round(Decimal.Divide(evArgs.TransferredBytes, evArgs.TotalBytes) * 100, 2);
                    });

                }
                decimal _currentProgress = Media.Sum(item => item.Progress);
                decimal _totalProgress = Media.Count * 100;
                ProgressPercentage = Math.Round(Decimal.Divide(_currentProgress, _totalProgress) * 100, 2);
                ProgressPercentageDisplay = ProgressPercentage + "%";
            };
            UserName = SettingsService.Instance.LoggedUserDetails.UserName;
        }
        async Task Handle_PostCommand()
        {

            try
            {
                if (IsNormalIndicator)
                    return;
                if (PostContent == null || PostContent == string.Empty)
                {
                    IsBusy = false;
                    IsNormalIndicator = false;
                    await ShowAlert(AppResources.AppName, AppResources.CreatePostAlertText);
                    return;
                }

                AddPostRequestInfo addPostRequestInfo = new AddPostRequestInfo();
                addPostRequestInfo.imageUrl = new List<ImageData>();
                addPostRequestInfo.videoUrl = new List<VideoData>();
                addPostRequestInfo.userId = SettingsService.Instance.LoggedUserDetails.UserId;
                addPostRequestInfo.postContent = PostContent;
                if (Media?.Count > 0)
                {
                   await AddAttachments(addPostRequestInfo);
                }
                else
                {
                    IsNormalIndicator = true;
                    var Response = await TailDataServiceProvider.Instance.AddPost(addPostRequestInfo);
                    Debug.WriteLine(Response.Message);
                    IsBusy = false;
                    IsNormalIndicator = false;
                    if (Response.ErrorCode != 200)
                    {
                        await ShowAlert(AppResources.AppName, Response.Message);
                    }
                    else
                    {
                        CommonSingletonUtility.SharedInstance.IsNewPostAdded = true;
                        Handle_BackCommand();
                    }
                        
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsNormalIndicator = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsBusy = false;
            IsNormalIndicator = false;
        }
        private async Task AddAttachments(AddPostRequestInfo addPostRequestInfo)
        {
            IsBusy = true;
            if (!CrossConnectivity.Current.IsConnected)
            {
                IsBusy = false;
                return;
            }
            if (await UploadImages())
            {
               await UplaodedImage(addPostRequestInfo);
            }
            else
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, AppResources.UploadFailedText);
                foreach (MediaFile image in Media)
                {
                    image.IsUploading = false;
                }
            }
        }
        private async Task UplaodedImage(AddPostRequestInfo addPostRequestInfo)
        {
            var imagesArray = Media.Where(x => x.Type == MediaFileType.Image).ToList();
            var videosArray = Media.Where(x => x.Type == MediaFileType.Video).ToList();

            if (imagesArray.Count > 0)
            {
                foreach (var imgItem in imagesArray)
                {
                    ImageData imgData = new ImageData();
                    imgData.fileUrl = imgItem.UploadedName;
                    imgData.fileText = string.Empty;
                    addPostRequestInfo.imageUrl.Add(imgData);
                }
            }
            if (videosArray.Count > 0)
            {
                foreach (var vidItem in videosArray)
                {
                    VideoData vidData = new VideoData();
                    vidData.fileUrl = vidItem.UploadedName;
                    vidData.fileText = string.Empty;
                    addPostRequestInfo.videoUrl.Add(vidData);
                }
            }
            var Response = await TailDataServiceProvider.Instance.AddPost(addPostRequestInfo);
            Debug.WriteLine(Response);
            if (Response.ErrorCode != 200)
            {
                await ShowAlert(AppResources.AppName, Response.Message);
            }
            else
            {
                CommonSingletonUtility.SharedInstance.IsNewPostAdded = true;
                Handle_BackCommand();
            }
            IsBusy = false;
            IsNormalIndicator = false;
        }
        async Task Handle_PostAPickCommand()
        {

            await NavigationService.NavigateWithInTabToAsync<PostYourPick>();
        }

        public override void Handle_BackCommand()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            SettingsService.Instance.CurrentTabIndex = 0;
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            if (currentTabbedPage != null)
                currentTabbedPage.CurrentPage = currentTabbedPage.Children[0];
            base.Handle_BackCommand();
        }
        void InitUpload()
        {
            foreach (MediaFile image in Media)
            {
                image.IsUploading = true;
            }

        }
        async Task<bool> UploadImages()
        {
            try
            {
                
                int numberOfBatch = Constants.IMAGE_UPLOAD_BATCH_COUNT;
                int numberOfItemsInBatch = Media.Count / numberOfBatch;
                List<Task<bool>> uploadTasks = new List<Task<bool>>();

                if (numberOfItemsInBatch > 0)
                {
                    List<List<MediaFile>> uploadBatchImages = new List<List<MediaFile>>();
                    int batchStartIndex = 0;
                    for (int i = 0; i < numberOfBatch; i++)
                    {
                        uploadBatchImages.Add(new List<MediaFile>());
                        for (int k = batchStartIndex; k < batchStartIndex + numberOfItemsInBatch; k++)
                        {
                            uploadBatchImages[i].Add(Media[k]);
                        }
                        batchStartIndex += numberOfItemsInBatch;
                    }

                    int remainingItems = Media.Count % numberOfBatch;

                    for (int j = 0; j < remainingItems; j++)
                    {
                        uploadBatchImages[j].Add(Media[batchStartIndex]);
                        batchStartIndex++;
                    }
                    
                    uploadTasks.Add(UploadImageBatchOne(uploadBatchImages[0], false));
                    uploadTasks.Add(UploadImageBatchTwo(uploadBatchImages[1], false));
                    uploadTasks.Add(UploadImageBatchThree(uploadBatchImages[2], false));
                }
                else
                {
                    numberOfBatch = Media.Count % numberOfBatch;

                    switch (numberOfBatch)
                    {
                        case 1:
                            uploadTasks.Add(UploadImageOne(Media[0]));
                            break;

                        case 2:
                            uploadTasks.Add(UploadImageOne(Media[0]));
                            uploadTasks.Add(UploadImageTwo(Media[1]));
                            break;

                        default:
                            break;
                    }
                }
                InitUpload();

                bool[] uploadTaskStatusList = await Task.WhenAll(uploadTasks);

                CurrentProgressCount = Media.Count;
                ProgressPercentage = (CurrentProgressCount / Media.Count) * 100;
                ProgressPercentageDisplay = ProgressPercentage + "%";

                foreach (bool uploadTaskStatus in uploadTaskStatusList)
                {
                    if (!uploadTaskStatus)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            return true;

        }

        async Task<bool> UploadImageBatchOne(List<MediaFile> images, bool batchStatus)
        {


            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                        batchStatus = true;
                    }

                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                            batchStatus = true;

                        }
                    
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                            batchStatus = true;
                        }
                       

                    }
                    UpdateUploadDone(images[i].ImageID);
                }

            }
            return batchStatus;
        }
        async Task<bool> UploadImageBatchTwo(List<MediaFile> images, bool batchStatus)
        {
            Debug.WriteLine("UploadImageBatchTwo");


            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                        batchStatus = true;
                    }
                   
                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                            batchStatus = true;

                        }
                        
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;

                            batchStatus = true;
                        }
                   

                    }
                    UpdateUploadDone(images[i].ImageID);
                }

            }


            return batchStatus;
        }

        async Task<bool> UploadImageBatchThree(List<MediaFile> images, bool batchStatus)
        {
            Debug.WriteLine("UploadImageBatchThree");


            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                        batchStatus = true;
                    }
                   

                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                            batchStatus = true;
                        }
                       
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                            batchStatus = true;
                        }
                       

                    }
                    UpdateUploadDone(images[i].ImageID);
                }

            }


            return batchStatus;
        }





        async Task<bool> UploadImageOne(MediaFile image)
        {
            bool uploadStatus = false;
            if (image.Type == MediaFileType.Image)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalImageName);
                if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, image.Path, Constants.S3BucketForPostImage, false))
                {
                    image.UploadedName = _orginalImageName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            else if (image.Type == MediaFileType.Video)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalVideoName = _keyName + ".mp4";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalVideoName);
                if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, image.Path, image.PreviewPath, Constants.S3BucketForPostVideo))
                {
                    image.UploadedName = _orginalVideoName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            return uploadStatus;
        }

        async Task<bool> UploadImageTwo(MediaFile image)
        {

            bool uploadStatus = false;
            if (image.Type == MediaFileType.Image)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_thumbImageName);
                Debug.WriteLine(_orginalImageName);
                if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, image.Path, Constants.S3BucketForPostImage, false))
                {
                    image.UploadedName = _orginalImageName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            else if (image.Type == MediaFileType.Video)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalVideoName = _keyName + ".mp4";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalVideoName);
                if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, image.Path, image.PreviewPath, Constants.S3BucketForPostVideo))
                {
                    image.UploadedName = _orginalVideoName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            return uploadStatus;
        }

        void UpdateUploadDone(int imageID)
        {
            for (int i = 0; i < Media.Count; i++)
            {
                if (Media[i].ImageID == imageID)
                {
                    Media[i].IsUploading = false;
                    break;
                }
            }

        }
    }
}
