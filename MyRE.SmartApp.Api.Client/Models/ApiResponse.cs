using System.Net;
using Optional;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public Option<ApiError> Error { get; set; } = Option.None<ApiError>();
    }
}
