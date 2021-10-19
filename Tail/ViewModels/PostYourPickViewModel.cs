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
    public class PostYourPickViewModel : PageViewModelBase
    {

        #region private members

        int _position;
        int _imageSelectedCount;
        double _pageHeight = 300;

        bool _isBackEnable;
        bool _isValid;
        bool _initialLaunch;
        bool _isNormalIndicator;


        string _userImage;
        string _userName;
        string _postContent;

        int _currentProgressCount = 1;
        decimal _progressPercentage = 0;
        string _progressCountDisplay;
        string _progressPercentageDisplay;

        ObservableCollection<MediaFile> _media;
        ObservableCollection<StepsDetails> _stepsDataList;
        List<LeagueDetails> _leagueList;

        Command _next;
      
        Command<int> _positionChangedCommand;
        Command _pageBack;
        Command _post;
        Command _imageTapCommand;
        #endregion
        #region public members

        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value);

        }
        public int CurrentPosition { get; set; }
        public double PageHeight
        {
            get => _pageHeight;
            set => SetProperty(ref _pageHeight, value);

        }

        public bool IsBackEnable
        {
            get => _isBackEnable;
            set => SetProperty(ref _isBackEnable, value);

        }
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }
        public bool InitialLaunch
        {
            get => _initialLaunch;
            set => SetProperty(ref _initialLaunch, value);

        }

        public ObservableCollection<StepsDetails> StepsDataList
        {
            get => _stepsDataList;
            set => SetProperty(ref _stepsDataList, value);
        }
        public List<LeagueDetails> LeagueList
        {
            get => _leagueList;
            set => SetProperty(ref _leagueList, value);
        }

        public ObservableCollection<MediaFile> Media
        {
            get => _media;
            set => SetProperty(ref _media, value);
        }

        public int ImageSelectedCount
        {
            get => _imageSelectedCount;
            set => SetProperty(ref _imageSelectedCount, value);
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
        public string PostContent
        {
            get => _postContent;
            set => SetProperty(ref _postContent, value);
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
        public Command<int> PositionChangedCommand => _positionChangedCommand ?? (_positionChangedCommand = new Command<int>((position) => Handle_PositionChanged(position)));
        public Command Next => _next ?? (_next = new Command(async () => await Handle_Next()));
     
        public Command PageBack => _pageBack ?? (_pageBack = new Command(() => Handle_PageBack()));
        public Command Post => _post ?? (_post = new Command(async () => await Handle_Post()));
        public Command ImageTapCommand => _imageTapCommand ?? (_imageTapCommand = new Command(async (item) => await Handle_ImageTapCommandCommandAsync(item)));

        public Action OnPageSwitched
        {
            get;
            set;
        }

        #endregion

        public PostYourPickViewModel()
        {
            try
            {
                InitialLaunch = true;
                CurrentPosition = 0;
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
                    ProgressPercentage = Math.Round(Decimal.Divide(evArgs.TransferredBytes, evArgs.TotalBytes) * 100, 2);
                    ProgressPercentageDisplay = ProgressPercentage + "%";
                };
                UserName = SettingsService.Instance.LoggedUserDetails.UserName;

                Task.Run(async () => await GetGameDetails());


            }
            catch (Exception ex)
            {
                Task.Run(async () => await ShowAlert(AppResources.AppName, "Error While Open Post Your Pick. \nERROR : " + ex.Message));
            }


        }
        ///<summary>
        ///Getting Details of available league matches.
        ///</summary>
        private async Task<bool> GetLeagueDetails(int SportID)
        {
            bool hasSuccessResponse = false;
            try
            {
                LeagueList = new List<LeagueDetails>();
                var leagueResponse = await TailDataServiceProvider.Instance.GetLeagueDetails(SportID);
                if (leagueResponse.ErrorCode == 200 && leagueResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    LeagueList = new List<LeagueDetails>(leagueResponse.ResponseData.SportsDetails);
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, leagueResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;
        }
        private async Task<bool> GetGameSchedules(int LeagueId, string EventDate)
        {
            bool hasSuccessResponse = false;
            try
            {
               
                var gameResponse = await TailDataServiceProvider.Instance.GetGameSchedules(LeagueId, EventDate);
                if (gameResponse.ErrorCode == 200 && gameResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    StepsDataList[0].UpcomingGames = gameResponse.ResponseData;

                    foreach (ScheduleInfo gameInfo in StepsDataList[0].UpcomingGames[0].GameData)
                    {
                        foreach (var gameItem in gameInfo.Games)
                        {
                            gameItem.SelectedCommand = new Command<string>((item) => Handle_GameSelected(item));
                        }


                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, gameResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;
        }

        ///<summary>
        ///Override back button to correct tab navigation.
        ///</summary>
        public override void Handle_BackCommand()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            SettingsService.Instance.CurrentTabIndex = 0;
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            if (currentTabbedPage != null)
                currentTabbedPage.CurrentPage = currentTabbedPage.Children[0];
            base.Handle_BackCommand();
        }
        ///<summary>
        ///Get details of all available game in coming days on selected league.
        ///</summary>
        private async Task GetGameDetails()
        {
            try
            {

                if (IsBusy)
                    return;
                IsBusy = true;
                var dateSpotsResponse = await TailDataServiceProvider.Instance.GetGameDetails();
                if (dateSpotsResponse.ErrorCode == 0 && dateSpotsResponse.ResponseData != null)
                {
                    StepsDataList = new ObservableCollection<StepsDetails>(dateSpotsResponse.ResponseData);
                    if (StepsDataList != null && StepsDataList.Count > 0)
                    {
                        StepsDataList[0].InformationText = AppResources.SelectYourSpotInfo;
                        StepsDataList[1].SelectedTabIndex = 1;

                        StepsDataList[1].BettingDetails.EditCommand = new Command(() => Handle_PageBack());

                        StepsDataList[1].BettingDetails.MoneyLineTabSelectedCommand = new Command((item) => Handle_MoneyLineTabSelection());
                        StepsDataList[1].BettingDetails.SpreadTabSelectedCommand = new Command((item) => Handle_SpreadTabSelection());
                        StepsDataList[1].BettingDetails.OverUnderTabSelectedCommand = new Command((item) => Handle_OverUnderTabSelection());

                        StepsDataList[1].BettingDetails.MonyLine1SelectedCommand = new Command<string>((item) => Handle_MoneyLine1Selection(item));
                        StepsDataList[1].BettingDetails.MonyLine2SelectedCommand = new Command<string>((item) => Handle_MoneyLine2Selection(item));

                        StepsDataList[1].BettingDetails.Spread1SelectedCommand = new Command<string>((item) => Handle_Spread1Selection(item));
                        StepsDataList[1].BettingDetails.Spread2SelectedCommand = new Command<string>((item) => Handle_Spread2Selection(item));

                        StepsDataList[1].BettingDetails.OverUnder1SelectedCommand = new Command<string>((item) => Handle_OverUnder1Selection(item));
                        StepsDataList[1].BettingDetails.OverUnder2SelectedCommand = new Command<string>((item) => Handle_OverUnder2Selection(item));


                        StepsDataList[0].OnSpotIndexChanged += SpotDropDown_Changed;
                        StepsDataList[0].OnLeagueIndexChanged += LeagueDropdown_Change;
                    }
                }

                IsBackEnable = false;
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Get Game Details. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }

        }
        ///<summary>
        ///Spot change event.
        ///</summary>
        async void SpotDropDown_Changed()
        {
            if (StepsDataList[0] != null && StepsDataList[0].SpotSelectedIndex != -1)
            {
                StepsDataList[0].IsGameAvailable = false;
                StepsDataList[0].InformationText = AppResources.SelectYourLeagueInfo;
                int _typeID = StepsDataList[0].SpotSelectedIndex + 1;

                SportType _SportType = (SportType)_typeID;
                StepsDataList[1].BettingDetails.Sport_Type = _SportType;
                if (await GetLeagueDetails(_typeID))
                {
                    List<LeagueDetails> _leagueOptions = LeagueList;
                    if (_leagueOptions != null && _leagueOptions.Count > 0)
                    {
                        StepsDataList[0].LeagueOptions = new List<PickerItem>();
                        List<PickerItem> _listPicker = new List<PickerItem>();
                        foreach (var item in _leagueOptions)
                        {
                            _listPicker.Add(new PickerItem
                            {
                                ItemName = item.LeagueName
                            });

                        }
                        StepsDataList[0].LeagueOptions = new List<PickerItem>(_listPicker);
                    }
                }
                else
                {
                    StepsDataList[0].LeagueOptions = new List<PickerItem>();
                }
                ResetGameSelection();

            }
        }
        ///<summary>
        ///League change event.
        ///</summary>
        async void LeagueDropdown_Change()
        {
            try
            {


                if (StepsDataList[0].LeagueSelectedIndex != -1)
                {
                    
                    IsNormalIndicator = true;
                   
                    LeagueDetails _leagueItem = LeagueList[StepsDataList[0].LeagueSelectedIndex];
                    if (_leagueItem != null)
                    {
                        StepsDataList[1].BettingDetails.SelectedEventID = _leagueItem.LeagueID;
                        StepsDataList[1].BettingDetails.SelectedEventName = _leagueItem.LeagueName;
                        StepsDataList[1].BettingDetails.SelectedEventImage = _leagueItem.LeagueImage;
                    }
                    
                    if (_leagueItem != null && await GetGameSchedules(_leagueItem.LeagueID, "0"))
                    {         
                        if (StepsDataList[0].UpcomingGames[0].GameData.Count == 2)
                        {
                            StepsDataList[0].IsGameAvailable = true;
                            PageHeight = 720;
                            StepsDataList[0].InformationText = AppResources.GameSelectionWarning;
                        }
                        else if (StepsDataList[0].UpcomingGames[0].GameData.Count == 1)
                        {
                            StepsDataList[0].IsGameAvailable = true;
                            PageHeight = 690;
                            StepsDataList[0].InformationText = AppResources.GameSelectionWarning;
                        }
                        else if (StepsDataList[0].UpcomingGames[0].GameData.Count == 0)
                        {
                            StepsDataList[0].IsGameAvailable = false;
                            
                            StepsDataList[0].InformationText = AppResources.GameInfo;
                        }
                        else
                        {
                            StepsDataList[0].IsGameAvailable = true;
                            PageHeight = 900;
                            StepsDataList[0].InformationText = AppResources.GameSelectionWarning;
                        }
                        //Should Delete
                        if (StepsDataList[1].BettingDetails.Sport_Type == SportType.MMA)
                        {
                            foreach (var gamesData in StepsDataList[0].UpcomingGames[0].GameData)
                            {
                                foreach (var item in gamesData.Games)
                                {
                                    item.HomeTeamDetails.SportID = 7;
                                    item.AwayTeamDetails.SportID = 7;
                                }
                            }
                        }
                        else if (StepsDataList[1].BettingDetails.Sport_Type == SportType.Basketball && StepsDataList[0].UpcomingGames[0].GameData != null && StepsDataList[0].UpcomingGames[0].GameData.Count != 0 && _leagueItem.LeagueID == 4)
                        {
                                foreach (var gamesData in StepsDataList[0].UpcomingGames[0].GameData)
                                {
                                    foreach (var item in gamesData.Games)
                                    {
                                        item.HomeTeamDetails.SportID = 4;
                                        item.AwayTeamDetails.SportID = 4;
                                    }
                                }
                        }
                           
                    }
                    else
                    {
                        StepsDataList[0].IsGameAvailable = false;
                        PageHeight = 300;
                        StepsDataList[0].InformationText = AppResources.GameInfo;
                    }

                    ResetGameSelection();
                    IsNormalIndicator = false;
                }
            }
            catch
            {
                IsNormalIndicator = false;
            }
        }
        ///<summary>
        ///Switching Step1 and Step2.
        ///</summary>
        void Handle_PositionChanged(int position)
        {
            try
            {
                CurrentPosition = position;

            }
            catch (Exception ex)
            {
                Task.Run(async () => await ShowAlert(AppResources.AppName, "Error While Switch Pages. \nERROR : " + ex.Message));
            }
        }
        async Task Handle_Next()
        {
            if (!IsValid)
            {
                await ShowAlert(AppResources.AppName, StepsDataList[0].InformationText);
                return;
            }
            if (Position != 1)
            {

                Position = CurrentPosition + 1;
                IsBackEnable = true;
                OnPageSwitched?.Invoke();
            }
        }
        void Handle_PageBack()
        {
            if (Position != 0)
            {
                Position = CurrentPosition - 1;
                IsBackEnable = false;
                OnPageSwitched?.Invoke();
            }
        }
        async Task Handle_Post()
        {
            if (IsBusy)
                return;

           
            if (PostContent == null || PostContent == string.Empty)
            {
                await ShowAlert(AppResources.AppName, AppResources.CreatePostAlertText);
            }
            else
            {
                AddPickRequestInfo addPickRequestInfo = new AddPickRequestInfo();
                addPickRequestInfo.imageUrl = new List<ImageData>();
                addPickRequestInfo.videoUrl = new List<VideoData>();
                addPickRequestInfo.postContent = PostContent;

                addPickRequestInfo.userId = SettingsService.Instance.LoggedUserDetails.UserId;
                addPickRequestInfo.mainSportId = (int)StepsDataList[1].BettingDetails.Sport_Type;
                addPickRequestInfo.sportId = StepsDataList[1].BettingDetails.SelectedEventID;
                addPickRequestInfo.sName = StepsDataList[1].BettingDetails.SelectedEventName;
                addPickRequestInfo.eventId = StepsDataList[1].BettingDetails.SelectedGame.EventID;
                addPickRequestInfo.eventDate = StepsDataList[1].BettingDetails.SelectedGame.GameDateTime;
                addPickRequestInfo.htName = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName;
                addPickRequestInfo.htAbbr = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamNameShort;
                addPickRequestInfo.htLogo = (string.IsNullOrEmpty(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Tlogo)) ? "No Image" : StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Tlogo;
                addPickRequestInfo.atName = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;
                addPickRequestInfo.atAbbr = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamNameShort;
                addPickRequestInfo.atLogo = (string.IsNullOrEmpty(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Tlogo)) ? "No Image" : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Tlogo;
                addPickRequestInfo.pickFee = StepsDataList[1].BettingDetails.PickPrice;
                addPickRequestInfo.betType = StepsDataList[1].SelectedTabIndex;
                if (StepsDataList[1].SelectedTabIndex == 1)
                {
                    if(StepsDataList[1].BettingDetails.MoneyLineSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }

                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.MoneylineDisply);
                    addPickRequestInfo.aMoScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.MoneylineDisply);
                    addPickRequestInfo.hSpScore = 0;
                    addPickRequestInfo.aSpScore = 0;
                    addPickRequestInfo.oScore = 0;
                    addPickRequestInfo.uScore = 0;

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? 2 : 1;

                }
                else if (StepsDataList[1].SelectedTabIndex == 2)
                {
                    if (StepsDataList[1].BettingDetails.SpreadSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }
                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = 0;
                    addPickRequestInfo.aMoScore =0;
                    addPickRequestInfo.hSpScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Spread);
                    addPickRequestInfo.aSpScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Spread);
                    addPickRequestInfo.oScore = 0;
                    addPickRequestInfo.uScore = 0;

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? 2 : 1;
                }
                else if (StepsDataList[1].SelectedTabIndex == 3)
                {
                    if (StepsDataList[1].BettingDetails.OverUnderSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }
                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = 0;
                    addPickRequestInfo.aMoScore = 0;
                    addPickRequestInfo.hSpScore = 0;
                    addPickRequestInfo.aSpScore = 0;
                    addPickRequestInfo.oScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.OverScore);
                    addPickRequestInfo.uScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.UnderScore);

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? 2 : 1;
                }

                if (Media?.Count > 0)
                {
                    IsBusy = true;
                    ProgressCountDisplay = CurrentProgressCount + "/" + Media.Count;
                    ProgressPercentageDisplay = "0%";
                    if (await UploadImages())
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
                                addPickRequestInfo.imageUrl.Add(imgData);
                            }
                        }
                        if (videosArray.Count > 0)
                        {
                            foreach (var vidItem in videosArray)
                            {
                                VideoData vidData = new VideoData();
                                vidData.fileUrl = vidItem.UploadedName;
                                vidData.fileText = string.Empty;
                                addPickRequestInfo.videoUrl.Add(vidData);
                            }
                        }
                        // Call final Post Webservice from here
                        var Response = await TailDataServiceProvider.Instance.AddPick(addPickRequestInfo);
                        if (Response.ErrorCode != 200)
                            await ShowAlert(AppResources.AppName, Response.Message);
                        else
                            Handle_BackCommand();

                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;

                        // Show error message here 
                        await ShowAlert(AppResources.AppName, AppResources.UploadFailedText);
                        foreach (MediaFile image in Media)
                        {
                            image.IsUploading = false;
                        }
                    }
                }
                else
                {
                    IsNormalIndicator = true;
                    var Response = await TailDataServiceProvider.Instance.AddPick(addPickRequestInfo);
                    IsBusy = false;
                    IsNormalIndicator = false;
                    if (Response.ErrorCode != 200)
                        await ShowAlert(AppResources.AppName, Response.Message);
                    else
                        Handle_BackCommand();


                }
            }
            IsBusy = false;

        }
        public void Handle_GameSelected(string GameID)
        {

            ChangeGameSelection(GameID);
        }

        public void Handle_MoneyLineTabSelection()
        {
            StepsDataList[1].SelectedTabIndex = 1;
        }
        public void Handle_SpreadTabSelection()
        {
            StepsDataList[1].SelectedTabIndex = 2;
        }
        public void Handle_OverUnderTabSelection()
        {
            StepsDataList[1].SelectedTabIndex = 3;
        }
        public void Handle_MoneyLine1Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.MoneyLineSelection = 1;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;

        }
        public void Handle_MoneyLine2Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.MoneyLineSelection = 2;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;

        }
        public void Handle_Spread1Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.SpreadSelection = 1;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;

        }
        public void Handle_Spread2Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.SpreadSelection = 2;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;
        }

        public void Handle_OverUnder1Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.OverUnderSelection = 1;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;

        }
        public void Handle_OverUnder2Selection(string GameID)
        {
            StepsDataList[1].BettingDetails.OverUnderSelection = 2;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
        }
        ///<summary>
        ///Select one game from available list.
        ///</summary>
        void ChangeGameSelection(string GameID)
        {

            foreach (ScheduleInfo gameInfo in StepsDataList[0].UpcomingGames[0].GameData)
            {
                gameInfo.GameRefresh = false;
                var _existingItem = gameInfo.Games.FirstOrDefault(p => p.Selected);
                if (_existingItem != null)
                    _existingItem.Selected = false;
                var _updateItem = gameInfo.Games.FirstOrDefault(p => p.EventID == GameID);
                if (_updateItem != null)
                {
                    _updateItem.Selected = true;
                    StepsDataList[1].BettingDetails.SelectedGame = _updateItem;

                }
                gameInfo.GameRefresh = true;
            }
            StepsDataList[0].InformationText = "";
            IsValid = true;
        }
        ///<summary>
        ///Reset previously selected game.
        ///</summary>
        void ResetGameSelection()
        {
            if (StepsDataList[0].UpcomingGames != null && StepsDataList[0].UpcomingGames[0].GameData != null)
            {
                foreach (ScheduleInfo gameInfo in StepsDataList[0].UpcomingGames[0].GameData)
                {
                    gameInfo.GameRefresh = false;

                    var _existingItem = gameInfo.Games.FirstOrDefault(p => p.Selected);
                    if (_existingItem != null)
                        _existingItem.Selected = false;

                    gameInfo.GameRefresh = true;
                 
                }
                IsValid = false;
               
            }
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
            int numberOfBatch = 1;
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

                uploadTasks.Add(UploadImageBatchOne(uploadBatchImages[0], true));
                
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

            foreach (bool uploadTaskStatus in uploadTaskStatusList)
            {
                if (!uploadTaskStatus)
                {
                    return false;
                }
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
                    Debug.WriteLine(_orginalImageName);
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
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
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

                    }



                
                    UpdateUploadDone(images[i].ImageID);
                }

            }
            if (images.Count > 1)
            {
                List<MediaFile> remaingingImages = new List<MediaFile>();
                for (int i = 1; i < images.Count; i++)
                {
                    remaingingImages.Add(new MediaFile
                    {
                        ImageID = images[i].ImageID,
                        Path = images[i].Path
                    });
                }
                await UploadImageBatchOne(remaingingImages, batchStatus);
            }

            return batchStatus;
        }

        async Task<bool> UploadImageBatchTwo(List<MediaFile> images, bool batchStatus)
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
                    Debug.WriteLine(_orginalImageName);
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                    }
                    else
                        batchStatus = false;
                   
                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                    {
                        images[i].UploadedName = _orginalVideoName;
                    }
                    else
                        batchStatus = false;
                  
                    UpdateUploadDone(images[i].ImageID);
                }
            }
            if (images.Count > 1)
            {
                List<MediaFile> remaingingImages = new List<MediaFile>();
                for (int i = 1; i < images.Count; i++)
                {
                    remaingingImages.Add(new MediaFile
                    {
                        ImageID = images[i].ImageID,
                        Path = images[i].Path
                    });
                }
                await UploadImageBatchTwo(remaingingImages, batchStatus);
            }
            return batchStatus;
        }

        async Task<bool> UploadImageBatchThree(List<MediaFile> images, bool batchStatus)
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
                    Debug.WriteLine(_orginalImageName);
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                    }
                    else
                        batchStatus = false;
                   
                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                    {
                        images[i].UploadedName = _orginalVideoName;
                    }
                    else
                        batchStatus = false;
                
                    UpdateUploadDone(images[i].ImageID);
                }
            }
            if (images.Count > 1)
            {
                List<MediaFile> remaingingImages = new List<MediaFile>();
                for (int i = 1; i < images.Count; i++)
                {
                    remaingingImages.Add(new MediaFile
                    {
                        ImageID = images[i].ImageID,
                        Path = images[i].Path
                    });
                }
                await UploadImageBatchThree(remaingingImages, batchStatus);
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

        private async Task Handle_ImageTapCommandCommandAsync(object item)
        {
            var tappedItem = item as MediaFile;
            if (tappedItem.Type == MediaFileType.Image)
                await PopupNavigation.Instance.PushAsync(new PostImagePopup(tappedItem.Path));
            else if (tappedItem.Type == MediaFileType.Video)
                await PopupNavigation.Instance.PushAsync(new PostVideoPopup(tappedItem.Path));

        }

    }
}