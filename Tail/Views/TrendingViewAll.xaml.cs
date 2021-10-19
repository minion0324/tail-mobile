using System;
using System.Diagnostics;
using Tail.ViewModels;

namespace Tail.Views
{
    public partial class TrendingViewAll : AppPageBase
    {
        TrendingViewAllViewModel _vModel;
        public TrendingViewAll()
        {
            try
            {
                InitializeComponent();
                _vModel = new TrendingViewAllViewModel();
                BindingContext = _vModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
          
        }
    }
}
