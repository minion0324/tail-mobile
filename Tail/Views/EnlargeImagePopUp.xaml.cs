using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Models;
using Tail.Services.Helper;

namespace Tail.Views
{
    public partial class EnlargeImagePopUp : PopupPage
    {
       
        public EnlargeImagePopUp(PostAttchment currentItem, List<PostAttchment> ImageSet)
        {
            InitializeComponent();
            List<PostAttchment> _tempImageSet = new List<PostAttchment>();

            foreach (var item in ImageSet)
            {
                if (item.FileType == 1)
                {
                    _tempImageSet.Add(item);
                }
            }

            if (_tempImageSet.Count == 1)
            {
              
                var urlPath = string.Empty;
                if (!string.IsNullOrEmpty(_tempImageSet[0].ImageName))
                {
                    urlPath = TailUtils.GetOrginalPostImage(_tempImageSet[0].ImageName);
                }
                else
                {
                    urlPath = "";
                }
                ImageViewSingle.Source = urlPath;
                ImageSlider.IsVisible = false;
                IndicatorView.IsVisible = false;
                SingleImageGrid.IsVisible = true;
            }
            else
            {
                ImageSlider.ItemsSource = _tempImageSet;
                ImageSlider.CurrentItem = currentItem;
                ImageSlider.IsVisible = true;
                IndicatorView.IsVisible = true;
                SingleImageGrid.IsVisible = false;
            }

           
          

        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
