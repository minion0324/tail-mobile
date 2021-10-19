using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.InAppBilling;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.Services.OnlineServices
{
    public class InAppPurchaseService
    {
        static InAppPurchaseService _instance = null;

        public static InAppPurchaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InAppPurchaseService();
                }
                return _instance;
            }
        }
        public async Task<InAppPurchaseResponse> PurchaseCoinPackAsync(string productId)
        {
            InAppPurchaseResponse inApppurchaseResponse = new InAppPurchaseResponse();
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {

                    return inApppurchaseResponse;
                }


                var verifyPurchase = DependencyService.Get<IInAppBillingVerifyPurchase>();
                string[] products = new string[1];
                products[0] = productId;
                CrossInAppBilling.Current.InTestingMode = true;
                var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, verifyPurchase);
                if (purchase == null)
                {
                    //Not purchased, alert the user
                    inApppurchaseResponse.Message = "Purchase Failed.";
                    inApppurchaseResponse.IsSuccess = false;
                    return inApppurchaseResponse;
                }
                else
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await CrossInAppBilling.Current.ConsumePurchaseAsync(productId, purchase.PurchaseToken);
                    }
                    inApppurchaseResponse.PurchaseId = purchase.Id;
                    inApppurchaseResponse.PurchaseToken = purchase.PurchaseToken;
                    inApppurchaseResponse.State = purchase.State;
                    inApppurchaseResponse.IsSuccess = true;
                    inApppurchaseResponse.Message = "Purchase Successfull";
                    return inApppurchaseResponse;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Something bad has occurred, alert user
                var message = string.Empty;
                switch (purchaseEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "This service is currently not available. Please try again later. ";
                        Debug.WriteLine(message);
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "The billing information is not available. Please try again later.";
                        Debug.WriteLine(message);
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "Could not process the payment. Please try again later.";
                        Debug.WriteLine(message);
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "You are not allowed to make the payment. ";
                        Debug.WriteLine(message);
                        break;
                    case PurchaseError.GeneralError:
                        message = "Could not process the payment request. Please try again later.";
                        Debug.WriteLine(message);
                        break;
                    case PurchaseError.UserCancelled:
                        message = "Your purchase has been canceled.";
                        Debug.WriteLine(message);
                        break;
                }


                //Decide if it is an error we care about
                if (string.IsNullOrWhiteSpace(message))
                    message = "Error in purchasing coins. Please try again later.";
                inApppurchaseResponse.Message = message;
                inApppurchaseResponse.IsSuccess = false;
                return inApppurchaseResponse;
                //Display message to user

            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                inApppurchaseResponse.Message = ex.Message;
                inApppurchaseResponse.IsSuccess = false;
                Debug.WriteLine("Issue connecting: " + ex);
                return inApppurchaseResponse;
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }
        public async Task<InAppProductResponse> GetCoinPacksFromAppleAsync()
        {
            IEnumerable<InAppBillingProduct> _inAppProducts = null;
            InAppProductResponse response = new InAppProductResponse();

            string[] products = new string[5];
            if (Device.RuntimePlatform == Device.iOS)
            {
                products[0] = "testTenPack";
                products[1] = "testTwentyPack";
                products[2] = "testThirtyPack";
                products[3] = "testFourtyPack";
                products[4] = "testFiftyPack";
            }
            else
            {
                products[0] = "tenpack";
                products[1] = "twentypack";
                products[2] = "thirtypack";
                products[3] = "fortypack";
                products[4] = "fiftypack";
            }

            try
            {
                var iapConnected = await CrossInAppBilling.Current.ConnectAsync();
                if (!iapConnected)
                {
                    response.ErrorCode = 2001;
                    response.Message = "This service is currently not available. Please try again later.";
                }
                else
                {
                    _inAppProducts = await CrossInAppBilling.Current.GetProductInfoAsync(ItemType.InAppPurchase, products);

                    if (_inAppProducts != null)
                    {
                        response.ErrorCode = 200;
                        response.products = _inAppProducts;
                        response.Message = "Products Fetched.";
                    }
                    else
                    {
                        response.ErrorCode = 3000;
                        response.Message = "Could not display the products. Please try again later.";
                    }
                }
            }
            catch (InAppBillingPurchaseException iapEx)
            {
                switch (iapEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        response.ErrorCode = 2002;
                        response.Message = "Currently the app store seems to be unavailble. Try again later.";
                        break;
                    case PurchaseError.InvalidProduct:
                        response.ErrorCode = 2003;
                        response.Message = "Could not fetch the details of the coin packs. Please try again later.";
                        break;
                    case PurchaseError.ProductRequestFailed:
                        response.ErrorCode = 2004;
                        response.Message = "Could not fetch the details of the coin packs. Please try again later.";
                        break;
                    default:
                        response.ErrorCode = 3000;
                        response.Message = "Could not fetch the details of the coin packs. Please try again later.";
                        break;
                }
            }
            catch
            {
                response.ErrorCode = 3000;
                response.Message = "Could not fetch the details of the coin packs. Please try again later..";
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
            return response;
        }
    }
}
