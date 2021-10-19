using System;
using System.Linq;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Common.Apis;
using Tail.Common;
using Tail.Services.Helper;

namespace Tail.Droid.Helpers
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { SmsRetriever.SmsRetrievedAction })]
    public class SmsReceiver : BroadcastReceiver
    {
        private static readonly string[] OtpMessageBodyKeywordSet = { "Your TAIL App verification code is" }; //You must define your own Keywords  
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {

                if (intent.Action != SmsRetriever.SmsRetrievedAction) return;
                var bundle = intent.Extras;
                if (bundle == null) return;
                var status = (Statuses)bundle.Get(SmsRetriever.ExtraStatus);
                switch (status.StatusCode)
                {
                    case CommonStatusCodes.Success:
                        var message = (string)bundle.Get(SmsRetriever.ExtraSmsMessage);
                        var foundKeyword = OtpMessageBodyKeywordSet.Any(k => message.Contains(k));
                        if (!foundKeyword) return;
                        var code = ExtractNumber(message);
                        OtpUtilities.Notify(Events.SmsRecieved, code);
                        break;
                    case CommonStatusCodes.Timeout:
                        break;
                }

            }
            catch (System.Exception)
            {
                // ignored  
            }
        }
        private static string ExtractNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var number = Regex.Match(text, @"\d+").Value;
            return number;
        }
    }
}
