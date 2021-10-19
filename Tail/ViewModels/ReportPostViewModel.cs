using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ServiceProviders;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ReportPostViewModel : PageViewModelBase
    {

        #region private members
        bool _isPickPost;
        bool _isReportPost;
        string _enterIssueText;
        PostDetails _postItem;
        List<PickerItem> _issueOptions;
        Command _reportPostCommand;
        #endregion
        #region public members

        public bool IsPickPost
        {
            get => _isPickPost;
            set => SetProperty(ref _isPickPost, value);
        }
        public string EnterIssueText
        {
            get => _enterIssueText;
            set => SetProperty(ref _enterIssueText, value);
        }

        
        public bool IsReportPost
        {
            get => _isReportPost;
            set => SetProperty(ref _isReportPost, value);
        }
        int _issueSelectedIndex;
        public int IssueSelectedIndex
        {
            get => _issueSelectedIndex;
            set => SetProperty(ref _issueSelectedIndex, value);
        }

        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }

        public List<PickerItem> IssueOptions
        {
            get => _issueOptions;
            set => SetProperty(ref _issueOptions, value);
        }
        public Action OnIssueIndexChanged
        {
            get;
            set;
        }

        public Command ReportPostCommand => _reportPostCommand ?? (_reportPostCommand = new Command(async () => await Handle_ReportPostCommand()));

        #endregion

        public ReportPostViewModel()
        {
            IssueOptions = new List<PickerItem>{
               
                new PickerItem
                {
                    ItemName="Hateful post"
                },
                new PickerItem
                {
                    ItemName="Misleading"
                },
                new PickerItem
                {
                    ItemName="Spam"
                },
                 new PickerItem
                {
                    ItemName="Nudity"
                },
                new PickerItem
                {
                    ItemName="Violence"
                },
                new PickerItem
                {
                    ItemName="Harassment"
                },
                 new PickerItem
                {
                    ItemName="Suicide or Self injury"
                },
                new PickerItem
                {
                    ItemName="False News"
                },
                new PickerItem
                {
                    ItemName="Terrorism"
                },
                new PickerItem
                {
                    ItemName="Something else"
                }
            };
        
        }
        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {

                PostItem = (PostDetails)parameter;
                IsReportPost = true;
                IsPickPost = (PostItem.Post_Type == PostType.Pick);

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Initialize Report Post. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    
        async Task Handle_ReportPostCommand()
        {
            if (IssueSelectedIndex >= 0)
            {

                if (await ReportAPost())
                {
                    PostItem.IsReport = true;
                }
            }
            else
            {
                await NavigationService.ShowAlertAsync(AppResources.AppName, AppResources.SelectYourIssue);

            }


        }
        async Task<bool> ReportAPost()
        {
            bool hasSuccessResponse = false;

            try
            {
                ReportPostRequestInfo requestObj = new ReportPostRequestInfo()
                {
                   postedBy= PostItem.UserId,
                   postId=PostItem.PostId.ToString(),
                   reportType= IssueSelectedIndex+1,
                   reportText = EnterIssueText
                };
                var reportResponse = await TailDataServiceProvider.Instance.ReportAPost(requestObj);
                if (reportResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                    await NavigationService.ShowAlertAsync(AppResources.AppName, reportResponse.Message);
                    Back.Execute(null);
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, reportResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
    }
}
