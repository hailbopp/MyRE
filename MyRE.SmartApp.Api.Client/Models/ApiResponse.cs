using System.Net;
using System.Net.Http.Headers;
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
        public string Raw { get; set; }
        public T Data { get; set; }
        public HttpResponseHeaders Headers { get; set; }

        public Option<ApiError> Error { get; set; } = Option.None<ApiError>();

        public ApiError ErrorData => this.Error.ValueOr(() => null);
    }
}
