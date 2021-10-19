using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PayoutHistoryResponse : BaseModel
    {
        ObservableCollection<PayOutModel> _payOutHistories;
        [JsonProperty("resultData")]
        public ObservableCollection<PayOutModel> PayOutHistories
        {
            get => _payOutHistories;
            set => SetProperty(ref _payOutHistories, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }
}
