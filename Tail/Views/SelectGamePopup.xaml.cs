using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Models;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class SelectGamePopup : PopupPage
    {
        readonly SelectGamePopupViewModel _vModel;
        public SelectGamePopup(IList<GameSchedule> upcomingGames, Action popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new SelectGamePopupViewModel();
            BindingContext = _vModel;
            _vModel.UpcomingGames = upcomingGames;
        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        void InsideFrame_Tapped(System.Object sender, System.EventArgs e)
        {
            // do nothing
        }

    }
}
