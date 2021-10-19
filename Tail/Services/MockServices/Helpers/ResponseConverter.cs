using Tail.Services.Responses;
namespace Tail.Services.MockServices.Helpers
{
    public static class ResponseConverter
    {
        public static ServiceResponse<TResult> GetServiceResponse<TResult>(TResult result)
        {
            var response = new ServiceResponse<TResult>();
            response.ErrorCode = 0;
            response.ResponseData = result;
            return response;
        }
    }
}
