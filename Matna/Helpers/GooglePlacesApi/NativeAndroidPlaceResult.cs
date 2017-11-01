namespace Matna.Helpers.GooglePlacesApi
{
    /// <summary>
    /// Android result set
    /// </summary>
    public class NativeAndroidPlaceResult : IPlaceResult
    {
        ///<inheritdoc/>
        public string PlaceId { get; set; }
        ///<inheritdoc/>
        public string Description { get; set; }
        ///<inheritdoc />
        public string Subtitle { get; set; }
    }
}