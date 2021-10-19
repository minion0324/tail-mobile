using System.Threading.Tasks;
using Tail.Models;

namespace Tail.Services.Interfaces
{
	public interface IFacebookService
	{
		Task<FbAccount> GetAccountAsync(string accessToken);
		Task PostOnWallAsync(string accessToken, string message);
	}
}
