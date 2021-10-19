using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.InAppBilling;
using Tail.Models;
using Tail.Services.OnlineServices;
using Tail.Services.ServiceProviders;
using Xamarin.Forms;

[assembly: Dependency(typeof(InAppPurchaseVerificationManager))]
namespace Tail.Services.OnlineServices
{
    public class InAppPurchaseVerificationManager : IInAppBillingVerifyPurchase
    {
        public InAppPurchaseVerificationManager()
        {
        }

        public async Task<bool> VerifyPurchase(string signedData, string signature, string productId = null, string transactionId = null)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    VerifyPurchase verifyPurchase = new VerifyPurchase
                    {
                        signedData = signedData
                    };
                    var result = await TailDataServiceProvider.Instance.VerifyInappPurchase(verifyPurchase);

                    if (result.ErrorCode == 200)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    PurchaseSignedData purchaseSignedData = JsonConvert.DeserializeObject<PurchaseSignedData>(signedData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    VerifyPurchaseAndroid verifyPurchase = new VerifyPurchaseAndroid
                    {
                        signedData = signedData,
                        packageName = "com.tailnetwork.tailapp",
                        productId = purchaseSignedData.ProductID,
                        purchaseToken = purchaseSignedData.PurchaseToken,
                        orderId = purchaseSignedData.OrderID,
                        purchaseState = purchaseSignedData.PurchaseState,
                        purchaseTime = purchaseSignedData.PurchaseTimeTicksUtc

                    };
                    var result = await TailDataServiceProvider.Instance.VerifyInappPurchaseAndroid(verifyPurchase);

                    if (result.ErrorCode == 200)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
                   
               
            }
            catch
            {
                return false;
            }
        }
        
    }
}
