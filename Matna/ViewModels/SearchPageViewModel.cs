using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Matna.Models;
using Matna.Utils.Restful;
using Matna.Views;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        List<GoogleAutocompletePrediction> predictions = new List<GoogleAutocompletePrediction>();
        public List<GoogleAutocompletePrediction> Predictions
        {
            get
            {
                return predictions;
            }
            set
            {
                predictions = value;
                OnPropertyChanged("Predictions");
            }
        }

        string searchText = "";
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }
        
        public SearchPageViewModel()
        {
            MessagingCenter.Subscribe<SearchPage>(this, "OnSearchClicked", async (sender) =>
            {
                Predictions = await Restful.Inst.GoogleMapsPlaceAutocomplete(SearchText);
            });

            // Properties (Commands) initilization
            OnBackClicked = new Command(() => {
                MessagingCenter.Send(this, "HideSearch");
            });
            OnSearchClicked = new Command(async () =>
            {
                Predictions = await Restful.Inst.GoogleMapsPlaceAutocomplete(SearchText);
            });
            OnCancelClicked = new Command(() => {
                SearchText = "";
            });
        }

        public ICommand OnBackClicked { get; }
        public ICommand OnSearchClicked { get; }
        public ICommand OnCancelClicked { get; }
    }
}
