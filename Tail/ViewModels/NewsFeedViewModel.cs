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
using System.Text.RegularExpressions;
using System.Text;
using System.Net;

namespace Tail.ViewModels
{
    public class NewsFeedViewModel : PageViewModelBase
    {
        int todayOffset = 0;
        int dayBeforeOffset = 0;
        int limit = 6;
        DateTimeOffset currentDate;
        HttpClient httpclient;
        private ObservableRangeCollection<Data> _todayNews;
        public ObservableRangeCollection<Data> TodayNews { get { return _todayNews; } set { SetProperty(ref _todayNews, value); } }
        private ObservableRangeCollection<Data> _dayBeforeNews;
        public ObservableRangeCollection<Data> DayBeforeNews { get { return _dayBeforeNews; } set { SetProperty(ref _dayBeforeNews, value); } }
        private bool _isInitialLoading;
        public bool IsInitialLoading { get { return _isInitialLoading; } set { SetProperty(ref _isInitialLoading, value); } }
        public ICommand LoadMoreCommand { get; set; }
        public ICommand PositionChangedCommand { get; set; }
        public ICommand SelectedCommand { get; set; }
        public ICommand HideTodayPanelCommand { get; set; }
        public ICommand ExpandCollapseCommand { get; set; }
        public NewsFeedViewModel()
        {
            httpclient = new HttpClient();
            currentDate = DateTimeOffset.Now;
            LoadMoreCommand = new Command(async (list) => await LoadMore());
            SelectedCommand = new Command<object>(async (data) => await ExecuteSelectedItem(data));
            TodayNews = new ObservableRangeCollection<Data>();
            DayBeforeNews = new ObservableRangeCollection<Data>();
            PositionChangedCommand = new Command<int>(async (position) => await ExecutePositionChanged(position));
            ExpandCollapseCommand = new Command(() => ExpandCollapeHandler());
            //HideTodayPanelCommand = new Command(() => )
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

        private async Task ExecuteSelectedItem(object data)
        {
            try
            {
                await Browser.OpenAsync(((Data)data).Url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
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
            if (TopPanelVisibility)
            {
                TopPanelVisibility = false;
                TopHeight = new GridLength(0, GridUnitType.Absolute);
                Glyph = "\uf107";
            }
        }
        async Task<MediaStackResponse> GetMediaStack(int offset, int limit, DateTimeOffset dateTimeOffset)
        {
            var url = new Uri(String.Format("{0}?access_key={1}&offset={2}&limit={3}&date={4}&categories=sports&countries=us", Constants.MediaStackBaseUrl, Constants.MediaStackApiKey, offset, limit, dateTimeOffset.ToString("yyyy-MM-dd")));
            HttpResponseMessage res = await httpclient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    var response = await res.Content.ReadAsAsync<MediaStackResponse>();
                    response.ListData.ForEach(d =>
                    {
                        d.Title = WebUtility.HtmlDecode(d.Title);
                        d.Description = WebUtility.HtmlDecode(d.Description);
                    });
                    return response;
                }
                catch (Exception e)
                {
                    return new MediaStackResponse();
                }

            }
            return new MediaStackResponse();
        }

        private GridLength topHeight = GridLength.Star;
        public GridLength TopHeight { get { return topHeight; } set { SetProperty(ref topHeight, value); } }

        private bool topPanelVisibility = true;
        public bool TopPanelVisibility
        {
            get { return topPanelVisibility; }
            set { SetProperty(ref topPanelVisibility, value); }
        }

        private string glyph = "\uf106";
        public string Glyph
        {
            get { return glyph; }
            set { SetProperty(ref glyph, value); }
        }

        private void ExpandCollapeHandler()
        {
            if (TopHeight.IsStar)
            {
                TopPanelVisibility = false;
                TopHeight = new GridLength(0, GridUnitType.Absolute);
                Glyph = "\uf107";
            }
            else
            {
                TopPanelVisibility = true;
                TopHeight = new GridLength(1, GridUnitType.Star);
                Glyph = "\uf106";
            }
        }
    }
}
