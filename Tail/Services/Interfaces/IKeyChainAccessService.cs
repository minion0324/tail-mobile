
using Tail.Models;

namespace Tail.Services.Interfaces
{
    public interface IKeyChainAccessService
    {
        AppleAccount GetStoredAccount(string AppleUserKey);
        bool SaveDetailsToKeychain(AppleAccount accountDetails);
    }
}
