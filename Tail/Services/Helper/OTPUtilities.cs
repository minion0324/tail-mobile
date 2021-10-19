using System;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Services.Helper
{
    public static class OtpUtilities
    {
        private static readonly object cc = new object();
        public static void Subscribe<TArgs>(this object subscriber, Events eventSubscribed, Action<TArgs> callBack)
        {
            MessagingCenter.Subscribe(subscriber, eventSubscribed.ToString(), new Action<object, TArgs>((e, a) => { callBack(a); }));
        }
        public static void Notify<TArgs>(Events eventNotified, TArgs argument)
        {
            MessagingCenter.Send(cc, eventNotified.ToString(), argument);
        }
    }
}
