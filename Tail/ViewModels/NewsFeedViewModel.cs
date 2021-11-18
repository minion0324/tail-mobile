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
            TodayNews.Add(new Data { Title = "Could This Be The Year We Finally Expand CFB Playoffs?", Image=new System.Uri("https://tailnetwork.blog/images/mg_collegefootballplayoff_1.jpg"), Url=new Uri("https://tailnetwork.blog/blog/could-this-the-year-we-finally-expand-cfb-playoffs/") });
            TodayNews.Add(new Data { Title = "The Good the Bad and the Unbettable", Image = new Uri("https://tailnetwork.blog/images/new-project.png"), Url = new Uri("https://tailnetwork.blog/blog/the.good-the-bad-and-the-unbettable.which-teams-have-been-the-most-and-least-reliable-ats/") });
            TodayNews.Add(new Data { Title = "The OBJ curse. Last shot to prove everyone wrong", Image = new Uri("https://tailnetwork.blog/images/hf8euaebeyk909zgiddb.jpg"), Url = new Uri("https://tailnetwork.blog/blog/the-obj-curse.one-last-shot-to-prove-everyone-wrong/") });
            TodayNews.Add(new Data { Title = "Decentralization has been a buzzword lately. Here's how it ties into gambling", Image = new Uri("https://tailnetwork.blog/images/casino.jpg"), Url = new Uri("https://tailnetwork.blog/blog/decentralized-has-been-a-buzzword-lately.here-s-how-it-ties-into-gambling/") });

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
                    Glyph = "\uf107";
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
