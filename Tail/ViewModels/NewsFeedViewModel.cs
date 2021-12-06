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
        int dayBeforeOffset = 1;
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
        public ICommand CarouselItemTapped { get; set; }
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
            CarouselItemTapped = new Command(async (item) =>
            {
                if (!string.IsNullOrEmpty(item.ToString()))
                    await Browser.OpenAsync(item.ToString(), BrowserLaunchMode.SystemPreferred);
            });
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
            if (data != null)
                await Browser.OpenAsync(((Data)data).Url, BrowserLaunchMode.SystemPreferred);
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
            //var TodayResponse = await GetMediaStack(todayOffset, limit, DateTimeOffset.Now);
            //TodayNews = new ObservableRangeCollection<Data>(TodayResponse.ListData);
            TodayNews = new ObservableRangeCollection<Data>();
            TodayNews.Add(new Data { Title = "Ballsgate: Is the MLB trying to alter game outcomes?", Image = new System.Uri("https://tailnetwork.blog/images/unnamed-1.jpg"), Url = new Uri("https://tailnetwork.blog/blog/ballsgate-is-the-mlb-trying-to-alter-game-outcomes/"), Description = "It seems that someone in the MLB inner circle has screwed the pooch, and let out a morsel of sensitive information. By now you probably heard that the MLB used two different types of balls last season." });
            TodayNews.Add(new Data { Title = "If you are a sports team that plans on cheating, do it now. Your window is closing", Image = new Uri("https://tailnetwork.blog/images/f6c4b52c-659a-411a-b8d4-11ff366c8637.jpg"), Url = new Uri("https://tailnetwork.blog/blog/if-you-are-a-sports-team-that-plans-on-cheating-do-it-now.your-window-is-closing/"), Description = "We have seen what seems to be a lot of sports controversy as of late, and a lot of it is focused on the lack of punishment that is going on. We all know the stories. Astros steal signs, and nothing happens." });
            TodayNews.Add(new Data { Title = "Tail Network. WTF is it.", Image = new Uri("https://tailnetwork.blog/images/unnamed.png"), Url = new Uri("https://tailnetwork.blog/blog/tail-network.wtf-is-it/"), Description = "When people ask me what it is that I do, it’s a difficult question to answer. That’s because most people who are asking me what I do are drunk blonde girls at Jamesons pub in Santa Monica California, and trying to explain sports gambling social networks to my disappointed mother at the thanksgiving table is hard enough." });

            var DayBeforeResponse = await GetMediaStack(dayBeforeOffset, limit, DateTimeOffset.Now.AddDays(-1));
            DayBeforeNews = new ObservableRangeCollection<Data>(DayBeforeResponse.ListData);
            dayBeforeOffset += limit;
        }

        bool waitFlag = false;
        private async Task LoadMore()
        {
            if (!waitFlag)
            {
                waitFlag = true;
                var response = await GetMediaStack(dayBeforeOffset, limit, DateTimeOffset.Now.AddDays(-1));
                DayBeforeNews.AddRange(response.ListData);
                dayBeforeOffset += limit;

                if (TopPanelVisibility)
                {
                    TopPanelVisibility = false;
                    TopHeight = new GridLength(0, GridUnitType.Absolute);
                    Glyph = "DownArrow.png";
                }
                waitFlag = false;
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

        private string glyph = "UpArrow.png";
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
                Glyph = "DownArrow.png";
            }
            else
            {
                TopPanelVisibility = true;
                TopHeight = new GridLength(1, GridUnitType.Star);
                Glyph = "UpArrow.png";
            }
        }
    }
}
