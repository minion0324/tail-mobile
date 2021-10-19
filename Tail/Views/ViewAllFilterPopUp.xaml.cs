using System;
using Rg.Plugins.Popup.Pages;
using Tail.Common;
using Tail.Models;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class ViewAllFilterPopUp : PopupPage
    {
        ViewAllFilterPopUpViewModel _vModel;
        public ViewAllFilterPopUp(FilterReturnAgrument previousSelectedList, Action<FilterReturnAgrument> popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new ViewAllFilterPopUpViewModel();
            _vModel.PopupCloseCallback = popUpCloseCallback;

            BindingContext = _vModel;
            if (previousSelectedList != null && _vModel.SportsListList != null)
            {
                foreach (var item in _vModel.SportsListList)
                {
                    foreach (var prevItem in previousSelectedList.SelectedSportInfo)
                    {
                        if (item.SportsInfo.SportType == prevItem)
                        {
                            item.SportsInfo.IsSelected = true;
                            item.SportsInfo.CheckboxImage = Constants.CHECKBOX_SELECTED;
                        }
                    }

                }

            }

            if (previousSelectedList != null && _vModel.PickTypeList != null)
            {
                foreach (var item in _vModel.PickTypeList)
                {
                    foreach (var prevItem in previousSelectedList.SelectedPurchaseInfo)
                    {
                        if (item.PickInfo.Pick_Type == prevItem)
                        {
                            item.PickInfo.IsSelected = true;
                            item.PickInfo.CheckboxImage = Constants.CHECKBOX_SELECTED;
                        }
                    }
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is ViewAllFilterPopUpViewModel)
            {
                _vModel = BindingContext as ViewAllFilterPopUpViewModel;

                _vModel.OnSportFilterUpdated += () =>
                {
                    FilterStack.Refresh();
                };
                _vModel.OnPickTypeFilterUpdated += () =>
                {
                    PickTypeStack.Refresh();
                };
            }
        }

    }
}
