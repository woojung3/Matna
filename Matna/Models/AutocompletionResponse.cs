using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Matna.Models
{
    public class GoogleMapsPlaceAutocomplete
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("predictions")]
        public List<GoogleAutocompletePrediction> Predictions { get; set; }
    }
    public class GoogleAutocompletePrediction
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("structured_formatting")]
        public StructuredFormatting StructuredFormatting { get; set; }

        public string FormattedText
        {
            get
            {
                if (StructuredFormatting.MainText == "")
                {
                    if (Description == "")
                        return "";
                    else
                        return Description;
                }
                else if (StructuredFormatting.SecondaryText == "")
                    return $"{StructuredFormatting.MainText}";
                else
                    return $"{StructuredFormatting.MainText} ({StructuredFormatting.SecondaryText})";
            }
        }
    }
    public class StructuredFormatting
    {
        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }
}
