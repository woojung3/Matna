using System;
using Xamarin.Forms;

namespace Matna.Helpers
{
    public class Keys
    {
        // Static Not working on Each Package
        public static string GoogleMapsApiKey = Device.RuntimePlatform == Device.iOS ? "AIzaSyA8BcmY3pnak84Ocgxg64blYPMuUwvE0Ek" :
                                                      Device.RuntimePlatform == Device.Android ? "AIzaSyDopEK8cvjMy-kPM5gN2077ZIuwcQEl_W8" :
                                                      "AIzaSyCvZhFK7yVPpzd8MEpfYy87nxyiyxHvcoc";
        // Use const instead
        public const string GoogleMapsApiKeyiOS = "AIzaSyA8BcmY3pnak84Ocgxg64blYPMuUwvE0Ek";
        public const string GoogleMapsApiKeyAndroid = "AIzaSyDopEK8cvjMy-kPM5gN2077ZIuwcQEl_W8";
        
        public static string GooglePlacesApiKey = Device.RuntimePlatform == Device.iOS ? "AIzaSyD1E5OX4GCikXVWj-GkZtUgObHHoh1kZyE" :
                                                        Device.RuntimePlatform == Device.Android ? "AIzaSyAN4fPxASLEQwYZShs9gJ9RR_gb79VH48M" :
                                                        "AIzaSyDYXQGTlMEuq3dikegiVT_2Lg7YQuyXUlI";
    }
}
