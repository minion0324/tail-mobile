using System;
using System.Diagnostics;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class Last15PredictionsView : ContentView
    {


        public Last15PredictionsView(MyProfileViewModel _viewModel)
        {

            try
            {
                InitializeComponent();
                this.BindingContext = _viewModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
