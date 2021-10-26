using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Plugin.Connectivity;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.Services.ServiceProviders;
using Tail.Validators;
using Tail.Validators.Rules;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ContactUsViewModel : PageViewModelBase
    {
        #region private members
        readonly  IMultiMediaPickerService _multiMediaPickerService;
        bool _attachmentVisible = false;
        Command _sendMessageCommand;
        Command _galleryCommand;

        int _currentProgressCount = 1;
        decimal _progressPercentage = 0;
        string _progressCountDisplay;
        string _progressPercentageDisplay;
        bool _progressVisible;
        #endregion
        #region public members

        public int SelectedCount
        {
            get;
            set;
        }
        public double TotalSelectedSize
        {
            get;
            set;
        }
        public bool SizeExceedPOpupShown { get; set; }
        public bool IsPopped { get; set; }
        public ObservableCollection<MediaFile> SelectedMedia
        {
            get;
            set;
        }
        public bool AttachmentVisible
        {
            get => _attachmentVisible;
            set => SetProperty(ref _attachmentVisible, value);
        }
        public ValidatableObject<string> TextContent { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> TextSubject { get; set; } = new ValidatableObject<string>();


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
        public bool ProgressVisible
        {
            get => _progressVisible;
            set => SetProperty(ref _progressVisible, value);
        }
        
        public Command GalleryCommand => _galleryCommand ?? (_galleryCommand = new Command(async () => await Handle_GalleryCommand()));
        public Command SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new Command(async () => await Handle_SendMessageCommand()));
        public Action OnSelectedImagesUpdated
        {
            get;
            set;
        }
        #endregion



        public ContactUsViewModel()
        {

            SelectedMedia = new ObservableCollection<MediaFile>();
            _multiMediaPickerService = CommonSingletonUtility.SharedInstance.MultiMediaPicker;
            _multiMediaPickerService.OnCancelled += (s, a) =>
            {
                IsBusy = false;
            };
            TotalSelectedSize = 0;
            SelectedCount = 0;
            IsPopped = false;
            DependencyService.Get<IAwsBucketService>().OnProgress += (s, e) =>
            {
                UploadProgressArgs evArgs = (UploadProgressArgs)e;
                ProgressPercentage = Math.Round(Decimal.Divide(evArgs.TransferredBytes, evArgs.TotalBytes) * 100, 2);
                ProgressPercentageDisplay = ProgressPercentage + "%";
            };
            _multiMediaPickerService.OnMediaPicked += (s, a) => ImageService_ImagePickedCompleted(s, a);
            if (SelectedMedia.Count > 0)
                AttachmentVisible = true;
            else
                AttachmentVisible = false;
            AddValidationRules();
        }
        public void AddValidationRules()
        {
            TextSubject.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.QuerySubjectValidation });
            TextContent.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.QueryContentValidation });
        }
        public void ImageService_ImagePickedCompleted(object s, MediaFile a)
        {
            if (IsPopped)
                return;
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (SelectedMedia.Count > 10)
                    return;
                FileInfo imageInfo = new FileInfo(a.Path);
                double sizeInMB = (double)imageInfo.Length / (1000 * 1000);
                TotalSelectedSize += sizeInMB;
               
                if (TotalSelectedSize > 50 && !SizeExceedPOpupShown)
                {
                    SizeExceedPOpupShown = true;
                    await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.UPloadSizeExeedsAlertText, AppResources.OKText);
                    return;
                }
                SelectedCount += 1;
                SelectedMedia.Add(a);
               
                OnSelectedImagesUpdated?.Invoke();
                if (SelectedMedia.Count > 0)
                    AttachmentVisible = true;
                else
                    AttachmentVisible = false;

            });

        }
        async Task Handle_GalleryCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            SizeExceedPOpupShown = false;
            double size = 0;
            if (SelectedMedia.Count > 0)
            {
                foreach (var file in SelectedMedia)
                {
                    FileInfo imageInfo = new FileInfo(file.Path);
                    double sizeInMB = (double)imageInfo.Length / (1000 * 1000);
                    size += sizeInMB;
                }
                TotalSelectedSize = size;
            }
            if (SelectedMedia.Count == 0)
                TotalSelectedSize = 0;
            if (SelectedMedia.Count >= 10)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.AttchmentLimitAlertText, AppResources.OKText);
                return;
            }
            if (size > 50)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.UPloadSizeExeedsAlertText, AppResources.OKText);
                return;
            }



            var hasPermission = await CheckGalleryPermissionsAsync();
            if (hasPermission)
            {
                await _multiMediaPickerService.PickPhotosAsync();
            }
            IsBusy = false;
        }

        async Task Handle_SendMessageCommand()
        {
             TextSubject.Validate();
             TextContent.Validate();
            try
            {

                if (!IsValid())
                    return;

                IsBusy = true;
                ContactUsRequestInfo contactUsRequestInfo = new ContactUsRequestInfo();
                contactUsRequestInfo.imageUrl = new List<ImageData>();
                contactUsRequestInfo.query = TextContent.Value;
                contactUsRequestInfo.subject = TextSubject.Value;
                if (SelectedMedia?.Count > 0)
                {
                    await AddMedia(contactUsRequestInfo);
                }
                else
                {
                    var Response = await TailDataServiceProvider.Instance.ContactUs(contactUsRequestInfo);

                    IsBusy = false;
                    ProgressVisible = false;
                    if (Response.ErrorCode != 200)
                        await ShowAlert(AppResources.AppName, Response.Message);
                    if (Response.ErrorCode == 200)
                        Handle_BackCommand();
                }

            }
            catch 
            {
                ProgressVisible = false;
                IsBusy = false;
            }

        }
        private async Task AddMedia(ContactUsRequestInfo contactUsRequestInfo)
        {
            ProgressVisible = true;

            ProgressCountDisplay = CurrentProgressCount + "/" + SelectedMedia.Count;
            ProgressPercentageDisplay = "0%";
            if (await UploadImages())
            {
                await UplaodedImage(contactUsRequestInfo);
            }
            else
            {
                IsBusy = false;
                ProgressVisible = false;
                // Show error message here 
                await ShowAlert(AppResources.AppName, AppResources.UploadFailedText);
                foreach (MediaFile image in SelectedMedia)
                {
                    image.IsUploading = false;
                }
            }
        }
        private async Task UplaodedImage(ContactUsRequestInfo contactUsRequestInfo)
        {
            var imagesArray = SelectedMedia.Where(x => x.Type == MediaFileType.Image).ToList();
            if (imagesArray.Count > 0)
            {
                foreach (var imgItem in imagesArray)
                {
                    ImageData imgData = new ImageData();
                    imgData.fileUrl = imgItem.UploadedName;
                    imgData.fileText = string.Empty;
                    contactUsRequestInfo.imageUrl.Add(imgData);
                }
            }

            // Call final Post Webservice from here
            var Response = await TailDataServiceProvider.Instance.ContactUs(contactUsRequestInfo);

            if (Response.ErrorCode != 200)
                await ShowAlert(AppResources.AppName, Response.Message);
            Handle_BackCommand();
            ProgressVisible = false;
            IsBusy = false;
        }
        private bool IsValid()
        {
            bool _isValid = false;
            if (!string.IsNullOrEmpty(TextSubject.Value) && !string.IsNullOrEmpty(TextContent.Value))
            {
                _isValid = true;
            }
            return _isValid;
        }
        async Task<bool> UploadImages()
        {
            try
            {

                int numberOfBatch = 1;
                int numberOfItemsInBatch = SelectedMedia.Count / numberOfBatch;
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
                            uploadBatchImages[i].Add(SelectedMedia[k]);
                        }
                        batchStartIndex += numberOfItemsInBatch;
                    }

                    int remainingItems = SelectedMedia.Count % numberOfBatch;

                    for (int j = 0; j < remainingItems; j++)
                    {
                        uploadBatchImages[j].Add(SelectedMedia[batchStartIndex]);
                        batchStartIndex++;
                    }

                    uploadTasks.Add(UploadImageBatchOne(uploadBatchImages[0], true));
                }
                InitUpload();

                bool[] uploadTaskStatusList = await Task.WhenAll(uploadTasks);

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
            if (!CrossConnectivity.Current.IsConnected)
            {
                IsBusy = false;
                return false;
            }

            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {
                   
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                        CurrentProgressCount += 1;
                        if (CurrentProgressCount <= images.Count)
                        {
                            ProgressCountDisplay = CurrentProgressCount + "/" + images.Count;
                            ProgressPercentage = 0;
                            ProgressPercentageDisplay = "0%";
                        }
                    }
                    else
                        batchStatus = false;
                   
                    UpdateUploadDone(images[i].ImageID);
                }


            }

            return batchStatus;
        }

        void UpdateUploadDone(int imageID)
        {
            for (int i = 0; i < SelectedMedia.Count; i++)
            {
                if (SelectedMedia[i].ImageID == imageID)
                {
                    SelectedMedia[i].IsUploading = false;
                    break;
                }
            }

        }
        void InitUpload()
        {
            foreach (MediaFile image in SelectedMedia)
            {
                image.IsUploading = true;
            }

        }

    }
}
