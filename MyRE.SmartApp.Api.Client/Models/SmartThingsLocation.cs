using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class SmartThingsLocation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ContactBookEnabled { get; set; }
        public IEnumerable<string> AvailableModes { get; set; }
        public string ActiveMode { get; set; }
        public string TempScale { get; set; }
        public long TimeZoneStandardOffsetMs { get; set; }
        public string PostalCode { get; set; }
        public GeoPosition Position { get; set; }
    }
}