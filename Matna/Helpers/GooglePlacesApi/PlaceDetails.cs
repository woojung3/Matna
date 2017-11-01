using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace Matna.Helpers.GooglePlacesApi
{
    /// <summary>
    /// Details of a place
    /// </summary>
    public class PlaceDetails
    {
        /// <summary>
        /// The Place Id
        /// </summary>
        public string PlaceId { get; set; }
        /// <summary>
        /// The Address as formatted text
        /// </summary>
        public string FormattedAddress { get; set; }
        /// <summary>
        /// The international phone number
        /// </summary>
        public string InternationalPhoneNumber { get; set; }
        /// <summary>
        /// The website uri
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Gets/Sets the coordinate of the place
        /// </summary>
        public Position Coordinate { get; set; }
        /// <summary>
        /// Gets/Sets the name of the place
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets/Sets the rating of the place
        /// </summary>
        public double? Rating { get; set; }
        /// <summary>
        /// Gets/Sets the types of the place
        /// </summary>
        public List<string> Types { get; set; }
    }
}