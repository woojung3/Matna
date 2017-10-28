using System;
using SQLite;

namespace Matna.Models
{
    public class PlaceItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Zoom { get; set; }

        public string ImageUrl { get; set; }
        public string Header
        {
            get
            {
                return Name;
            }
        }
    }
}
