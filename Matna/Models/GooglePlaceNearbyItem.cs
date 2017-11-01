using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Matna.Helpers;

namespace Matna.Models
{
    public class GooglePlaceNearbys
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public List<GooglePlaceNearbyItem> Results { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }
    }

    public class GooglePlaceNearbyItem
    {
        [PrimaryKey, JsonProperty("place_id")]
        public string PlaceId { get; set; }

        bool isSaved = false;
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
            }
        }
        public bool IsNotSaved
        {
            get
            {
                return !isSaved;
            }
        }

        [Ignore, JsonProperty("types")]
        public List<string> Types { get; set; }

        [Ignore, JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        double lat = 0;
        public double Lat
        {
            get
            {
                if (Math.Abs(lat) < 0.0001)
                {
                    lat = Geometry.Location.Lat;
                }
                return lat;
            }
            set
            {
                lat = value;
            }
        }
        double lon = 0;
        public double Lon
        {
            get
            {
                if (Math.Abs(lon) < 0.0001)
                {
                    lon = Geometry.Location.Lon;
                }
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }

        [Ignore, JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
        public string IconUrl
        {
            get
            {
                if (Icon != null)
                    return Icon;
                else return PropertiesDictionary.IconDefaultImage;
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }
        public double RatingD
        {
            get
            {
                if (Rating != null)
                    return (double)Rating;
                else
                    return 0.0;
            }
        }
        public bool IsStar1Visible
        {
            get
            {
                if (RatingD > 0.75)
                    return true;
                else return false;
            }
        }
        public bool IsStar2Visible
        {
            get
            {
                if (RatingD >= 1.75)
                    return true;
                else return false;
            }
        }
        public bool IsStar3Visible
        {
            get
            {
                if (RatingD >= 2.75)
                    return true;
                else return false;
            }
        }
        public bool IsStar4Visible
        {
            get
            {
                if (RatingD >= 3.75)
                    return true;
                else return false;
            }
        }
        public bool IsStar5Visible
        {
            get
            {
                if (RatingD >= 4.75)
                    return true;
                else return false;
            }
        }
        public bool IsHalfStarVisible
        {
            get
            {
                if (RatingD < 0.5)
                    return false;
                if (Math.Abs(Math.Round(RatingD) - RatingD) > 0.25 && Math.Abs(Math.Round(RatingD) - RatingD) < 0.75)
                    return true;
                else return false;
            }
        }

        [Ignore, JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }
        public bool IsOpen
        {
            get
            {
                if (OpeningHours == null)
                    return false;
                if (OpeningHours.OpenNow)
                    return true;
                return false;
            }
        }
        public bool IsClosed
        {
            get
            {
                if (OpeningHours == null)
                    return false;
                if (!OpeningHours.OpenNow)
                    return true;
                return false;
            }
        }

        [Ignore, JsonProperty("photos")]
        public List<Photo> Photos { get; set; }

        string imageUrl = "";
        public string ImageUrl
        {
            get
            {
                if (imageUrl != "")
                    return imageUrl;
                
                if (Photos != null && Photos.Any())
                {
                    if (Photos[0].PhotoReference != null)
                    {
                        imageUrl = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=80&photoreference={Photos[0].PhotoReference}&key={Keys.GooglePlacesApiKey}";
                        return imageUrl;
                    }
                }
                imageUrl = PropertiesDictionary.NotFoundImage;
                return imageUrl;
            }
            set
            {
                imageUrl = value;
            }
        }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lon { get; set; }
    }

    public class Photo
    {
        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }
    }

    public class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }
    }
}
