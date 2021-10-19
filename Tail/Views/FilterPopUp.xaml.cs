using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Tail.Common;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class FilterPopUp : PopupPage
    {
        FilterPopUpViewModel _vModel;
        public FilterPopUp(List<SportType> previousSelectedList, Action<List<SportType>> popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new FilterPopUpViewModel();
            _vModel.PopupCloseCallback = popUpCloseCallback;
          
            BindingContext = _vModel;
            if (previousSelectedList != null && _vModel.SportsListList != null)
            {
                foreach (var item in _vModel.SportsListList)
                {
                    foreach (var prevItem in previousSelectedList)
                    {
                        if (item.SportsInfo.SportType == prevItem)
                        {
                            item.SportsInfo.IsSelected = true;
                            item.SportsInfo.CheckboxImage = Constants.CHECKBOX_SELECTED;
                        }
                    }

                }
                var recordCount = _vModel.SportsListList.Count(a => a.SportsInfo.IsSelected);
                if (recordCount == 5)
                {
                    _vModel.SportsListList[recordCount].SportsInfo.IsSelected = true;
                    _vModel.SportsListList[recordCount].SportsInfo.CheckboxImage = Constants.CHECKBOX_SELECTED;
                }

            }

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is FilterPopUpViewModel)
            {
                _vModel = BindingContext as FilterPopUpViewModel;

                _vModel.OnSportFilterUpdated += () =>
                {


                    FilterStack.Refresh();

                };
            }
        }
    }
}
