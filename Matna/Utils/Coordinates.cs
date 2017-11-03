using System;
using System.Runtime.Serialization;

namespace Matna.Utils
{
    /// <summary>
    /// The coordinates class.
    /// </summary>
    [DataContract]
    public class Coordinates
    {
        /// <summary>
        /// The equator radius.
        /// </summary>
        public const int EquatorRadius = 6378137;

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        [DataMember]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        [DataMember]
        public double Longitude { get; set; }

        public Coordinates(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Calculates distance between two locations.
        /// </summary>
        /// <returns>The <see cref="System.Double"/>The distance in meters</returns>
        /// <param name="a">Location a</param>
        /// <param name="b">Location b</param>
        public static double DistanceBetween(Coordinates coord1, Coordinates coord2)
        {
            var dLat = coord2.Latitude * Math.PI / 180 - coord1.Latitude * Math.PI / 180;
            var dLon = coord2.Longitude * Math.PI / 180 - coord1.Longitude * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(coord1.Latitude * Math.PI / 180) * Math.Cos(coord2.Latitude * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = EquatorRadius * c;
            return d;
        }

        /// <summary>
        /// Calculates this locations distance to another coordicate.
        /// </summary>
        /// <returns>The distance to another coordicate</returns>
        /// <param name="other">Other coordinates.</param>
        public double DistanceFrom(Coordinates other)
        {
            return DistanceBetween(this, other);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0:0.0000}, {1:0.0000})", Latitude, Longitude);
        }
    }
}
