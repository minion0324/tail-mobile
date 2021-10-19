using Xamarin.Forms;
namespace Tail.Controls
{
    public class EntryWithoutBorder : Entry
    {
  
        public static readonly BindableProperty ClearButtonDisabledProperty = BindableProperty.Create("ClearButtonDisabled",
                                                                                                      typeof(bool),
                                                                                                      typeof(EntryWithoutBorder),
                                                                                                      false);

        public string ButtonType
        {
            get;
            set;
        }

        public bool ClearButtonDisabled
        {
            get => (bool)GetValue(ClearButtonDisabledProperty);
            set => SetValue(ClearButtonDisabledProperty, value);
        }

     
    }
}