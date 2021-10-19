using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class PostImagePopup : PopupPage
    {
        public PostImagePopup(string path)
        {
            InitializeComponent();
            ImageView.Source = ImageSource.FromFile(path);
        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
