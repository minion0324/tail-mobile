using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class OTPVerification : AppPageBase
    {
        readonly OtpVerificationViewModel _vModel;
        public OTPVerification()
        {
            InitializeComponent();
            _vModel = new OtpVerificationViewModel();
            _vModel.IsPageDestroyed = false;
            BindingContext = _vModel;
            OTPFirstEntry.Focused += (object sender, FocusEventArgs e) => { OTPFirstEntry.Text = ""; };
            OTPSecondEntry.Focused += (object sender, FocusEventArgs e) => { OTPSecondEntry.Text = ""; };
            OTPThridEntry.Focused += (object sender, FocusEventArgs e) => { OTPThridEntry.Text = ""; };
            OTPFourthEntry.Focused += (object sender, FocusEventArgs e) => { OTPFourthEntry.Text = ""; };
            OtpAutoFillEntry.Unfocused += (object sender, FocusEventArgs e) =>
            {
                OTPFirstEntry.BorderColor = Color.FromHex("#D8D8D8");
                OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
                OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
                OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
            };
            OtpAutoFillEntry.Focused += (object sender, FocusEventArgs e) =>
            {
                if(OTPFirstEntry.Text == null || OTPFirstEntry.Text.Length == 0)
                    OTPFirstEntry.BorderColor = Color.FromHex("#00FF00");
                else if(OTPSecondEntry.Text == null || OTPSecondEntry.Text.Length == 0)
                    OTPSecondEntry.BorderColor = Color.FromHex("#00FF00");
                else if (OTPThridEntry.Text == null || OTPThridEntry.Text.Length == 0)
                    OTPThridEntry.BorderColor = Color.FromHex("#00FF00");
                else if (OTPFourthEntry.Text == null || OTPFourthEntry.Text.Length == 0)
                    OTPFourthEntry.BorderColor = Color.FromHex("#00FF00");
            };
            this.Subscribe<string>(Events.SmsRecieved, code =>
            {
                if(!_vModel.IsPageDestroyed)
                {
                    OtpAutoFillEntry.Text = code;
                }
               
            });
            
            if(CommonSingletonUtility.SharedInstance.OtpPhoneNumber != "")
            {
                string phoneLast2Digit = CommonSingletonUtility.SharedInstance.OtpPhoneNumber.Substring(CommonSingletonUtility.SharedInstance.OtpPhoneNumber.Length - 2);
                OtpTitleLabel.Text = AppResources.OTPTitleText + phoneLast2Digit;
            }
           SetFocus();
        }
        async void SetFocus()
        {
            await Task.Delay(700);
            OtpAutoFillEntry.Focus();
            OTPFirstEntry.BorderColor = Color.FromHex("#00FF00");
            OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
            OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
            OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
        }
        
        void OTPFourthEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (OTPFourthEntry.Text.Length > 0)
            {
                OTPFourthEntry.Unfocus();
                _vModel.ValidateOTP();
            }
        }

        void OtpAutoFillEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var enteredText = e.NewTextValue;
            char[] charArray = enteredText.ToCharArray();
            if(charArray.Length == 0)
            {
                OTPFirstEntry.Text = "";
                OTPFirstEntry.BorderColor = Color.FromHex("#00FF00");
                OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
                OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
                OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
            }
            for (int i = 0; i < charArray.Length; i++)
            {
                switch (i)
                {
                    case 0: OTPFirstEntry.Text = charArray[i].ToString();
                        OTPSecondEntry.Text = "";
                        OTPFirstEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPSecondEntry.BorderColor = Color.FromHex("#00FF00");
                        OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
                        break;
                    case 1: OTPSecondEntry.Text = charArray[i].ToString();
                        OTPThridEntry.Text = "";
                        OTPFirstEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPThridEntry.BorderColor = Color.FromHex("#00FF00");
                        OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
                        break;
                    case 2:
                        OTPThridEntry.Text = charArray[i].ToString();
                        OTPFourthEntry.Text = "";
                        OTPFirstEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPFourthEntry.BorderColor = Color.FromHex("#00FF00");
                        break;
                    case 3:
                        OTPFourthEntry.Text = charArray[i].ToString();
                        OTPFirstEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPSecondEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPThridEntry.BorderColor = Color.FromHex("#D8D8D8");
                        OTPFourthEntry.BorderColor = Color.FromHex("#D8D8D8");
                        break;
                }
            }
        }
        public override void OnPageDestroy()
        {
            base.OnPageDestroy();
            _vModel.IsPageDestroyed = true;
        }
    }
}
