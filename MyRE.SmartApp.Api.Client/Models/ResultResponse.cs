using System;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class ResultResponse<T>
    {
        public T Result { get; set; }
    }
}