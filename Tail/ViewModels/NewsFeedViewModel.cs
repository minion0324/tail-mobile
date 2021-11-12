using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Tail.Common;
using Tail.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Tail.ViewModels
{
    public class NewsFeedViewModel : PageViewModelBase
    {
        int todayOffset = 0;
        int dayBeforeOffset = 0;
        int limit = 6;
        DateTimeOffset currentDate;
        HttpClient httpclient;
        public ObservableRangeCollection<Data> _todayNews;
        public ObservableRangeCollection<Data> TodayNews { get { return _todayNews; } set { SetProperty(ref _todayNews, value); } }
        public ObservableRangeCollection<Data> _dayBeforeNews;
        public ObservableRangeCollection<Data> DayBeforeNews { get { return _dayBeforeNews; } set { SetProperty(ref _dayBeforeNews, value); } }
        public bool _isInitialLoading;
        public bool IsInitialLoading { get { return _isInitialLoading; } set { SetProperty(ref _isInitialLoading, value); } }
        public ICommand LoadMoreCommand { get; set; }
        public ICommand PositionChangedCommand { get; set; }
        public ICommand SelectedCommand { get; set; }
        public NewsFeedViewModel()
        {
            httpclient = new HttpClient();
            currentDate = DateTimeOffset.Now;
            LoadMoreCommand = new Command(async (list) => await LoadMore());
            SelectedCommand = new Command<Data>(async (data) => await ExecuteSelectedItem(data));
            TodayNews = new ObservableRangeCollection<Data>();
            DayBeforeNews = new ObservableRangeCollection<Data>();
            PositionChangedCommand = new Command<int>(async (position) => await ExecutePositionChanged(position));
        }

        private async Task ExecutePositionChanged(int position)
        {
            if (position == TodayNews.Count - 1)
            {
                todayOffset += 1;
                var response = await GetMediaStack(todayOffset, limit, DateTimeOffset.Now);
                TodayNews.AddRange(response.ListData);
            }
        }
        private async Task ExecuteSelectedItem(Data data)
        {
            try
            {
                await Browser.OpenAsync(data.Url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex) { }
        }
        public override async void OnPageAppearing()
        {
            base.OnPageAppearing();
            if (TodayNews.Count == 0)
            {
                IsInitialLoading = true;
                await InitNewsFeed();
                IsInitialLoading = false;
            }

        }
        private async Task InitNewsFeed()
        {
            var TodayResponse = await GetMediaStack(todayOffset, limit, DateTimeOffset.Now);
            TodayNews = new ObservableRangeCollection<Data>(TodayResponse.ListData);
            var DayBeforeResponse = await GetMediaStack(dayBeforeOffset, limit, DateTimeOffset.Now.AddDays(-1));
            DayBeforeNews = new ObservableRangeCollection<Data>(DayBeforeResponse.ListData);
        }
        private async Task LoadMore()
        {
            dayBeforeOffset += 1;
            var response = await GetMediaStack(dayBeforeOffset, limit, DateTimeOffset.Now.AddDays(-1));
            DayBeforeNews.AddRange(response.ListData);
        }
        async Task<MediaStackResponse> GetMediaStack(int offset, int limit, DateTimeOffset dateTimeOffset)
        {
            var url = new Uri(String.Format("{0}?access_key={1}&offset={2}&limit={3}&date={4}&categories=sports&countries=us", Constants.MediaStackBaseUrl, Constants.MediaStackApiKey, offset, limit, dateTimeOffset.ToString("yyyy-MM-dd")));
            HttpResponseMessage res = await httpclient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    var response =  await res.Content.ReadAsAsync<MediaStackResponse>();
                    return response;
                }
                catch (Exception e)
                {
                    return new MediaStackResponse();
                }

            }
            return new MediaStackResponse();
        }


    }
}
