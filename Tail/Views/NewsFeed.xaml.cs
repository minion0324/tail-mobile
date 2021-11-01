using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class NewsFeed : AppPageBase
    {
        NewsFeedViewModel _vModel;

        public NewsFeed()
        {
            InitializeComponent();
            _vModel = new NewsFeedViewModel();
            BindingContext = _vModel;
        }
    }
}
