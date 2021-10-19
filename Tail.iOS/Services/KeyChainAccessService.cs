using System;
using Foundation;
using Security;
using Tail.iOS.Services;
using Tail.Models;
using Tail.Services.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(KeyChainAccessService))]
namespace Tail.iOS.Services
{
    public class KeyChainAccessService : IKeyChainAccessService
    {
        public AppleAccount GetStoredAccount(string AppleUserKey)
        {
            var rec = new SecRecord(SecKind.GenericPassword)
            {
                Generic = NSData.FromString(AppleUserKey)
            };
            var storedAccount = new AppleAccount();
            SecStatusCode res;
            var match = SecKeyChain.QueryAsRecord(rec, out res);
            if (res == SecStatusCode.Success)
            {
                storedAccount.FirstName = match.Label;
                storedAccount.LastName = match.Description;
                storedAccount.Email = match.Account;
                storedAccount.Token = match.Service;
                storedAccount.UserId = match.Comment;
            }
            else
            {
                storedAccount.FirstName = string.Empty;
                storedAccount.LastName = string.Empty;
                storedAccount.Email = string.Empty;
                storedAccount.Token = string.Empty;
                storedAccount.UserId = string.Empty;
            }
               

                return storedAccount;
        }

        public bool SaveDetailsToKeychain(AppleAccount accountDetails)
        {
            var s = new SecRecord(SecKind.GenericPassword)
            {
                Label = accountDetails.FirstName,
                Description = accountDetails.LastName,
                Account = accountDetails.Email,
                Service = accountDetails.Token,
                Comment = accountDetails.UserId,
                ValueData = NSData.FromString("my-secret-password"),
                Generic = NSData.FromString("AppleData")
            };
            var err = SecKeyChain.Add(s);

            if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
                return false;
            else
                return true;
        }
    }
}
