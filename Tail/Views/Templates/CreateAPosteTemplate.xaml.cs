using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class CreateAPosteTemplate : ContentView
    {
        IMultiMediaPickerService _multiMediaPickerService;
        public ObservableCollection<MediaFile> SelectedMedia
        {
            get => (ObservableCollection<MediaFile>)GetValue(SelectedMediaProperty);
            set => SetValue(SelectedMediaProperty, value);
        }
        public string EditorText
        {
            get => (string)GetValue(EditorTextProperty);
            set => SetValue(EditorTextProperty, value);
        }
        public bool IsPostAPick
        {
            get => (bool)GetValue(IsPostAPickProperty);
            set => SetValue(IsPostAPickProperty, value);
        }
        public int SelectedCount
        {
            get => (int)GetValue(SelectedCountProperty);
            set => SetValue(SelectedCountProperty, value);
        }
        public double TotalSelectedSize
        {
            get;
            set;
        }
        public bool IsBusy { get; set; }
        public bool SizeExceedPOpupShown { get; set; }
        public bool IsPopped { get; set; }
        public CreateAPosteTemplate()
        {
            InitializeComponent();
            HideCaptionStack.IsVisible = false;
            IsHideCaptionEnabled = false;
            if (!IsHideCaptionEnabled)
                HideCaptionStack.Opacity = 0.5;
            else
                HideCaptionStack.Opacity = 1;
            SelectedMedia = new ObservableCollection<MediaFile>();

            IsHideSelected = false;
            _multiMediaPickerService = CommonSingletonUtility.SharedInstance.MultiMediaPicker;


            _multiMediaPickerService.OnCancelled += (s, a) =>
            {
                IsBusy = false;
            };
            TotalSelectedSize = 0;
            SelectedCount = 0;
            IsPopped = false;
            _multiMediaPickerService.OnMediaPicked += (s, a) => ImageService_ImagePickedCompleted(s, a);


            if (SelectedMedia.Count > 0)
            {
                PickedImages.IsVisible = true;
                MediacountFrame.IsVisible = true;
                MediaCountText.Text = SelectedMedia.Count.ToString();
            }

            else
            {
                PickedImages.IsVisible = false;
                MediacountFrame.IsVisible = false;
            }
        }
        public void DeregisterOnMediaPicked()
        {
            IsPopped = true;



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
                BindableLayout.SetItemsSource(InterestList, SelectedMedia);
                if (SelectedMedia.Count > 0)
                {
                    PickedImages.IsVisible = true;
                    MediacountFrame.IsVisible = true;
                    MediaCountText.Text = SelectedMedia.Count.ToString();
                }

                else
                {
                    PickedImages.IsVisible = false;
                    MediacountFrame.IsVisible = false;
                }

            });

        }

        public Command AttachmentTapCommand
        {
            get => (Command)GetValue(AttachmentTapCommandProperty);
            set => SetValue(AttachmentTapCommandProperty, value);
        }
        public string ProfileName
        {
            get => (string)GetValue(ProfileNameProperty);
            set => SetValue(ProfileNameProperty, value);
        }

        public string ProfileImage
        {
            get => (string)GetValue(ProfileImageProperty);
            set => SetValue(ProfileImageProperty, value);
        }
        public bool IsShare
        {
            get => (bool)GetValue(IsShareProperty);
            set => SetValue(IsShareProperty, value);
        }
        public bool IsHideSelected
        {
            get => (bool)GetValue(IsHideSelectedProperty);
            set => SetValue(IsHideSelectedProperty, value);
        }
        public bool IsHideCaptionEnabled
        {
            get => (bool)GetValue(IsHideCaptionEnabledProperty);
            set => SetValue(IsHideCaptionEnabledProperty, value);
        }
        public Command ShareNowCommand
        {
            get => (Command)GetValue(ShareNowCommandProperty);
            set => SetValue(ShareNowCommandProperty, value);
        }
        public Command MoreOptionsCommand
        {
            get => (Command)GetValue(MoreOptionsCommandProperty);
            set => SetValue(MoreOptionsCommandProperty, value);
        }

        public static readonly BindableProperty AttachmentTapCommandProperty = BindableProperty.Create(propertyName: "AttachmentTapCommand",
                                                                            returnType: typeof(Command),
                                                                            declaringType: typeof(CreateAPosteTemplate));
        public static readonly BindableProperty ProfileNameProperty = BindableProperty.Create(propertyName: "ProfileName",
                                                                                            returnType: typeof(string),
                                                                                            declaringType: typeof(CreateAPosteTemplate),
                                                                                            defaultValue: "",
                                                                                            propertyChanged: OnNamePropertyChanged);
        public static readonly BindableProperty ProfileImageProperty = BindableProperty.Create(propertyName: "ProfileImage",
                                                                                            returnType: typeof(string),
                                                                                            declaringType: typeof(CreateAPosteTemplate),
                                                                                            defaultValue: "",
                                                                                            propertyChanged: OnProfileImagePropertyChanged);
        public static readonly BindableProperty IsShareProperty = BindableProperty.Create(propertyName: "IsShare",
                                                                                         returnType: typeof(bool),
                                                                                         declaringType: typeof(CreateAPosteTemplate),
                                                                                         defaultValue: false,
                                                                                         propertyChanged: OnIsSharePropertyChanged);
        public static readonly BindableProperty IsHideSelectedProperty = BindableProperty.Create(propertyName: "IsHideSelected",
                                                                                         returnType: typeof(bool),
                                                                                         declaringType: typeof(CreateAPosteTemplate),
                                                                                         defaultValue: false,
                                                                                         propertyChanged: OnIsHideSelectedPropertyChanged);

        public static readonly BindableProperty IsHideCaptionEnabledProperty = BindableProperty.Create(propertyName: "IsHideCaptionEnabled",
                                                                                         returnType: typeof(bool),
                                                                                         declaringType: typeof(CreateAPosteTemplate),
                                                                                         defaultValue: false,
                                                                                         propertyChanged: OnIsHideCaptionEnabledPropertyChanged);
        public static readonly BindableProperty SelectedMediaProperty = BindableProperty.Create(propertyName: "SelectedMedia",
          returnType: typeof(ObservableCollection<MediaFile>),
          declaringType: typeof(CreateAPosteTemplate));

        public static readonly BindableProperty SelectedCountProperty = BindableProperty.Create(propertyName: "SelectedCount",
          returnType: typeof(int),
          declaringType: typeof(CreateAPosteTemplate));

        public static readonly BindableProperty EditorTextProperty = BindableProperty.Create(propertyName: "EditorText",
                                                                                           returnType: typeof(string),
                                                                                           declaringType: typeof(CreateAPosteTemplate),
                                                                                           defaultValue: ""
                                                                                           );
        public static readonly BindableProperty IsPostAPickProperty = BindableProperty.Create(propertyName: "IsPostAPick",
                                                                                          returnType: typeof(bool),
                                                                                          declaringType: typeof(CreateAPosteTemplate),
                                                                                          defaultValue: false,
                                                                                          propertyChanged: OnIsPostAPickPropertyChanged
                                                                                          );


        public static readonly BindableProperty ShareNowCommandProperty = BindableProperty.Create(propertyName: "ShareNowCommand",
                                                                                          returnType: typeof(Command),
                                                                                          declaringType: typeof(CreateAPosteTemplate),
                                                                                          propertyChanged: OnShareNowCommandProperty);

        public static readonly BindableProperty MoreOptionsCommandProperty = BindableProperty.Create(propertyName: "MoreOptionsCommand",
                                                                                         returnType: typeof(Command),
                                                                                         declaringType: typeof(CreateAPosteTemplate),
                                                                                         propertyChanged: OnMoreOptionsCommandProperty);


        static void OnNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            

        }
        static void OnProfileImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
          

        }
        static void OnShareNowCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate _control = bindable as CreateAPosteTemplate;
            _control.ShareNowButton.Command = (Command)newValue;
        }
        static void OnMoreOptionsCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate _control = bindable as CreateAPosteTemplate;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Command = (Command)newValue;
            _control.FooterLeft.GestureRecognizers.Add(tapGestureRecognizer);
        }
        static void OnIsPostAPickPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate createAPosteTemplate = bindable as CreateAPosteTemplate;
            bool ispick = (bool)newValue;
            if (ispick)
                createAPosteTemplate.HideCaptionStack.IsVisible = true;
            else
                createAPosteTemplate.HideCaptionStack.IsVisible = false;
        }
        
        static void OnIsHideSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate createAPosteTemplate = bindable as CreateAPosteTemplate;
            bool isSelected = (bool)newValue;
            createAPosteTemplate.CheckBoxView.Source = isSelected ? "switch_on.png" : "switch_off.png";
        }
        static void OnIsHideCaptionEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate createAPosteTemplate = bindable as CreateAPosteTemplate;
            bool isEnabled = (bool)newValue;
            if (isEnabled)
                createAPosteTemplate.HideCaptionStack.Opacity = 1;
            else
                createAPosteTemplate.HideCaptionStack.Opacity = 0.5;
        }
        static void OnIsSharePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CreateAPosteTemplate createAPosteTemplate = bindable as CreateAPosteTemplate;
            bool _isShareScreen = (bool)newValue;
            if (_isShareScreen)
            {
                createAPosteTemplate.FooterRight.IsVisible = false;
                createAPosteTemplate.EditorView.Placeholder = AppResources.SharePlaceHolder;
                createAPosteTemplate.ShareNowButton.IsVisible = true;
                createAPosteTemplate.FooterLeft.IsVisible = true;
                createAPosteTemplate.MediaStack.IsVisible = false;
                createAPosteTemplate.PickedImages.IsVisible = false;
                createAPosteTemplate.EditorView.HeightRequest = 91;
                createAPosteTemplate.HeaderLabel.IsVisible = false;
            }
            else
            {
                createAPosteTemplate.ShareNowButton.IsVisible = false;
                createAPosteTemplate.EditorView.Placeholder = AppResources.WriteYourPost;
                createAPosteTemplate.FooterRight.IsVisible = true;
                createAPosteTemplate.FooterLeft.IsVisible = false;
                createAPosteTemplate.MediaStack.IsVisible = true;
                createAPosteTemplate.PickedImages.IsVisible = true;
                createAPosteTemplate.EditorView.HeightRequest = 30;
            }
        }

        async void FromGallery_Tapped(System.Object sender, System.EventArgs e)
        {
            if (IsBusy)
                return;
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
            IsBusy = true;
            Plugin.Permissions.Abstractions.PermissionStatus status;
            if (Device.RuntimePlatform == Device.iOS)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos) && Device.RuntimePlatform == Device.iOS)
                    {

                        await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.GalleryPermmission, AppResources.OKText);
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                    if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {

                        var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                        if (res) { CrossPermissions.Current.OpenAppSettings(); }
                    }
                }
            }
            else
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage) && Device.RuntimePlatform == Device.iOS)
                    {

                        await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.GalleryPermmission, AppResources.OKText);

                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {

                        var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                        if (res) { CrossPermissions.Current.OpenAppSettings(); }

                    }
                }
            }


            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var action = await App.Current.MainPage.DisplayActionSheet("Choose the type ", "Cancel", null, "Images", "Videos");
                switch (action)
                {
                    case "Images":
                        await _multiMediaPickerService.PickPhotosAsync();
                        break;
                    case "Videos":
                        await _multiMediaPickerService.PickVideosAsync();
                        break;
                    default:
                        break;
                }
            }
            IsBusy = false;
        }

        async void FromCamera_Tapped(System.Object sender, System.EventArgs e)
        {
            if (IsBusy)
                return;
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
                IsBusy = false;
                return;
            }
            if (size > 50)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.UPloadSizeExeedsAlertText, AppResources.OKText);
                IsBusy = false;
                return;
            }
            IsBusy = true;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();

            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera) && Device.RuntimePlatform == Device.iOS)
                {

                    await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.CameraPermmission, AppResources.OKText);

                }
                status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                {

                    var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                    if (res) { CrossPermissions.Current.OpenAppSettings(); }

                }
            }

            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var StoragePermissionstatus = Plugin.Permissions.Abstractions.PermissionStatus.Granted;
                if (Device.RuntimePlatform == Device.Android)
                {
                    StoragePermissionstatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (StoragePermissionstatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage) && Device.RuntimePlatform == Device.iOS)
                        {

                            await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.GalleryPermmission, AppResources.OKText);

                        }

                        StoragePermissionstatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        if (StoragePermissionstatus == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                        {

                            var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                            if (res) { CrossPermissions.Current.OpenAppSettings(); }

                        }
                    }
                }
                if (StoragePermissionstatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var action = await App.Current.MainPage.DisplayActionSheet("Choose the type ", "Cancel", null, "Image", "Video");
                    switch (action)
                    {
                        case "Image":
                            await PicImageFromCamera();
                            break;
                        case "Video":
                            await PicVideoFromCamera();
                            break;
                        default:
                            break;
                    }
                }
            }
            IsBusy = false;
        }

        void EditorView_Completed(System.Object sender, System.EventArgs e)
        {
            EditorText = EditorView.Text;
        }

        private async Task PicImageFromCamera()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", " No camera avaialble.", "OK");
            }
            await CrossMedia.Current.Initialize();


            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 60,
                Directory = "temp",
                Name = "Tail.jpg"
            });
            if (file != null)
            {
                var path = file.Path;
                var thumbPath = DependencyService.Get<IImageResizeHelper>().MaxResizeImage(path, 180, 120);
                Device.BeginInvokeOnMainThread(() =>
                {
                    SelectedMedia.Add(new MediaFile { Path = path, PreviewPath = thumbPath, Type = MediaFileType.Image });
                    BindableLayout.SetItemsSource(InterestList, SelectedMedia);
                    if (SelectedMedia.Count > 0)
                    {
                        PickedImages.IsVisible = true;
                        MediacountFrame.IsVisible = true;
                        MediaCountText.Text = SelectedMedia.Count.ToString();
                    }

                    else
                    {
                        PickedImages.IsVisible = false;
                        MediacountFrame.IsVisible = false;
                    }

                });
            }
            IsBusy = false;
        }
        private async Task PicVideoFromCamera()
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<MicrophonePermission>();

            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone) && Device.RuntimePlatform == Device.iOS)
                {

                    await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.MicroPhonePermmission, AppResources.OKText);

                }
                status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                {

                    var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                    if (res) { CrossPermissions.Current.OpenAppSettings(); }

                }
            }
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", " No camera avaialble.", "OK");
            }
            await CrossMedia.Current.Initialize();
            if (Device.RuntimePlatform == Device.Android)
            {
                var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    DesiredLength = TimeSpan.FromSeconds(25),
                    SaveToAlbum = true,
                    Quality = Plugin.Media.Abstractions.VideoQuality.Medium,
                });
                if (file != null)
                {
                    var path = file.Path;
                    FileInfo imageInfo = new FileInfo(file.Path);
                    double sizeInMB = (double)imageInfo.Length / (1000 * 1000);
                    TotalSelectedSize += sizeInMB;
                    if (TotalSelectedSize > 50)
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.UPloadSizeExeedsAlertText, AppResources.OKText);
                        return;
                    }
                    var thumbPath = DependencyService.Get<IImageHelper>().GetThumbnailFromVideo(path);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SelectedMedia.Add(new MediaFile { Path = path, PreviewPath = thumbPath, Type = MediaFileType.Video });
                        BindableLayout.SetItemsSource(InterestList, SelectedMedia);

                        if (SelectedMedia.Count > 0)
                        {
                            PickedImages.IsVisible = true;
                            MediacountFrame.IsVisible = true;
                            MediaCountText.Text = SelectedMedia.Count.ToString();
                        }

                        else
                        {
                            PickedImages.IsVisible = false;
                            MediacountFrame.IsVisible = false;
                        }
                    });
                }
            }
            else
            {
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        DesiredLength = TimeSpan.FromSeconds(25),
                        SaveToAlbum = true,
                        Quality = Plugin.Media.Abstractions.VideoQuality.Medium,
                    });
                    if (file != null)
                    {
                        var path = file.Path;
                        FileInfo imageInfo = new FileInfo(file.Path);
                        double sizeInMB = (double)imageInfo.Length / (1000 * 1000);
                        TotalSelectedSize += sizeInMB;
                        if (TotalSelectedSize > 50)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.UPloadSizeExeedsAlertText, AppResources.OKText);
                            return;
                        }

                        var thumbPath = DependencyService.Get<IImageHelper>().GetThumbnailFromVideo(path);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            SelectedMedia.Add(new MediaFile { Path = path, PreviewPath = thumbPath, Type = MediaFileType.Video });
                            BindableLayout.SetItemsSource(InterestList, SelectedMedia);
                            if (SelectedMedia.Count > 0)
                            {
                                PickedImages.IsVisible = true;
                                MediacountFrame.IsVisible = true;
                                MediaCountText.Text = SelectedMedia.Count.ToString();
                            }

                            else
                            {
                                PickedImages.IsVisible = false;
                                MediacountFrame.IsVisible = false;
                            }
                        });
                    }
                }
            }
        }

        async void MoreOption_Tapped(System.Object sender, System.EventArgs e)
        {

            await ShareUri();
        }

        public async Task ShareUri()
        {
            if (CommonSingletonUtility.SharedInstance.ShareImagePath != null)
                await Share.RequestAsync(new ShareFileRequest
                {
                    File = new ShareFile(CommonSingletonUtility.SharedInstance.ShareImagePath),
                    Title = "Look What it is!"

                });
        }

        void RemoveIcon_Tapped(System.Object sender, System.EventArgs e)
        {
            Image imgClicked = (Image)sender;
            var img = (TapGestureRecognizer)imgClicked.GestureRecognizers[0];
            var item = img.CommandParameter as MediaFile;
            SelectedMedia.Remove(SelectedMedia.FirstOrDefault(i => i.PreviewPath == item.PreviewPath));
            if (SelectedMedia.Count > 0)
            {
                PickedImages.IsVisible = true;
                MediacountFrame.IsVisible = true;
                MediaCountText.Text = SelectedMedia.Count.ToString();
            }

            else
            {
                PickedImages.IsVisible = false;
                MediacountFrame.IsVisible = false;
            }
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
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            Grid gridClicked = (Grid)sender;
            var itemGrid = (TapGestureRecognizer)gridClicked.GestureRecognizers[0];
            var item = itemGrid.CommandParameter as MediaFile;
            AttachmentTapCommand.Execute(item);
        }

        void HideCaption_Tapped(System.Object sender, System.EventArgs e)
        {
            IsHideSelected = !IsHideSelected;
            CheckBoxView.Source = IsHideSelected ? "switch_on.png" : "switch_off.png";
        }
    }
}
