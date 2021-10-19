using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;

namespace Tail.Models
{
    public class CommentsMain : BaseModel
    {
        IList<Comments> _commentsData;
        [JsonProperty("resultData")]
        public IList<Comments> CommentsData
        {
            get => _commentsData;
            set => SetProperty(ref _commentsData, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }
    public class PaginationDetails 
    {
        public int totalRecords { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }
    public class CommentRequestInfo
    {
        public string postId { get; set; }
        public string comment { get; set; }
        public string lastAddedOn { get; set; }
        
    }
    public class DeleteCommentRequestInfo
    {
        public string commentId { get; set; }
        public string postId { get; set; }
        
    }
    public class Comments : BaseModel
    {
        string _iD;
        [JsonProperty("_id")]
        public string ID
        {
            get => _iD;
            set => SetProperty(ref _iD, value);
        }
        string _postId;
        [JsonProperty("postId")]
        public string PostId
        {
            get => _postId;
            set => SetProperty(ref _postId, value);
        }
        int _userID;
        [JsonProperty("userId")]
        public int UserID
        {
            get => _userID;
            set => SetProperty(ref _userID, value);
        }

        [JsonIgnore]
        public bool EnableDelete
        {
            get
            {
                if (UserID==SettingsService.Instance.LoggedUserDetails.UserId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        string _userName;
        [JsonProperty("uName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        string _userImage;
        [JsonProperty("uImg")]
        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        int _status;
        [JsonProperty("uStatus")]
        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        [JsonIgnore]
        public string DisplayUserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(UserImage))
                {
                    return UserImage;
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }
            }
        }
        string _postedDateTime;
        [JsonProperty("addOn")]
        public string PostedDateTime
        {
            get => _postedDateTime;
            set => SetProperty(ref _postedDateTime, value);
        }
        [JsonIgnore]
        public string PostedTime
        {
            get
            {
                if (!string.IsNullOrEmpty(PostedDateTime))
                {
                    DateTime _postDateTime = Convert.ToDateTime(PostedDateTime);
                    return TailUtils.FindDisplayTime(_postDateTime);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        string _commentText;
        [JsonProperty("comment")]
        public string CommentText
        {
            get => _commentText;
            set => SetProperty(ref _commentText, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
        Command<string> _deleteCommand;
        public Command<string> DeleteCommand
        {
            get => _deleteCommand;
            set => SetProperty(ref _deleteCommand, value);
        }
       

        bool _isBusy=false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        bool _isSuccess=true;
        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }
    }
}
