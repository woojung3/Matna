using Xamarin.Forms;

namespace Matna.Helpers.GooglePlacesApi
{
    /// <summary>
    /// Manages instance of <see cref="IGooglePlacesApi"/>
    /// </summary>
    public static class GooglePlacesApi
    {
        private static IGooglePlacesApi _instance;

        /// <summary>
        /// Gets an instance of <see cref="IGooglePlacesApi"/>
        /// </summary>
        public static IGooglePlacesApi Instance
        {
            get
            {
                return _instance ?? (_instance = DependencyService.Get<IGooglePlacesApi>());
            }
        }
    }
}