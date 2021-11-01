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
        int offset;
        int limit;
        DateTimeOffset currentDate;
        HttpClient httpclient;
        public ObservableRangeCollection<Data> _togdayNews;
        public ObservableRangeCollection<Data> TodayNews { get { return _togdayNews; } set { SetProperty(ref _togdayNews, value); } }

        public Data _selectedItem;
        public Data SelectedItem { get { return _selectedItem; } set { SetProperty(ref _selectedItem, value); } }

        public bool _isInitialLoading;
        public bool IsInitialLoading { get { return _isInitialLoading; } set { SetProperty(ref _isInitialLoading, value); } }

        public ICommand LoadMoreCommand { get; set; }
        public ICommand SelectedCommand { get; set; }
        public NewsFeedViewModel()
        {
            httpclient = new HttpClient();
            offset = 0;
            limit = 6;
            currentDate = DateTimeOffset.Now;
            LoadMoreCommand = new Command(async () => await LoadMore());
            SelectedCommand = new Command(async () => await ExecuteSelectedItem(SelectedItem));
            TodayNews = new ObservableRangeCollection<Data>();


        }

        private async Task ExecuteSelectedItem(Data selectedItem)
        {
            try
            {
                await Browser.OpenAsync(selectedItem.Url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
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
            var response = await GetMediaStack(offset, limit, DateTimeOffset.Now);
            TodayNews = new ObservableRangeCollection<Data>(response.ListData);
        }

        private async Task LoadMore()
        {
            var response = await GetMediaStack(offset + 1, limit, DateTimeOffset.Now);
            TodayNews.AddRange(response.ListData);
        }
        async Task<MediaStackResponse> GetMediaStack(int offset, int limit, DateTimeOffset dateTimeOffset)
        {
            var url = new Uri(String.Format("{0}?access_key={1}&offset={2}&limit={3}&date={4}&categories=sports", Constants.MediaStackBaseUrl, Constants.MediaStackApiKey, offset, limit, dateTimeOffset.ToString("yyyy-MM-dd")));
            HttpResponseMessage res = await httpclient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    return await res.Content.ReadAsAsync<MediaStackResponse>();
                }
                catch (Exception e)
                {
                    return null;
                }

            }
            return null;
        }


    }
}
