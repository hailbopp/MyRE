using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRE.Web.Models.Web
{
    public class SmartThingsAuthBlob
    {
        public string AccessToken { get; set; }
        public string ApiServerBaseUrl { get; set; }
        public string AppId { get; set; }
        public string ExecutionToken { get; set; }
        public string AccountId { get; set; }
    }
}
