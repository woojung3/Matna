using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Matna.Models
{
    public class GooglePlaceDetails
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("result")]
        public GooglePlaceDetailsResult Result { get; set; }
    }
    public class GooglePlaceDetailsResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }
    }
    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}
