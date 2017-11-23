using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Matna.Helpers;
using Matna.Models;
using Matna.Utils.Restful;
using Matna.Views;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        ObservableCollection<GoogleAutocompletePrediction> predictions = new ObservableCollection<GoogleAutocompletePrediction>();
        public ObservableCollection<GoogleAutocompletePrediction> Predictions
        {
            get
            {
                if (SearchText == "")
                    return new ObservableCollection<GoogleAutocompletePrediction>(PropertiesDictionary.SearchHist);
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
            MessagingCenter.Unsubscribe<SearchPage>(this, "OnSearchClicked");
            MessagingCenter.Subscribe<SearchPage>(this, "OnSearchClicked", async (sender) =>
            {
                var rtn = await Restful.Inst.GoogleMapsPlaceAutocomplete(SearchText);
                Predictions = new ObservableCollection<GoogleAutocompletePrediction>(rtn);
            });

            // Properties (Commands) initilization
            OnBackClicked = new Command(() => {
                MessagingCenter.Send(this, "HideSearch");
            });
            OnSearchClicked = new Command(async () =>
            {
                var rtn = await Restful.Inst.GoogleMapsPlaceAutocomplete(SearchText);
                Predictions = new ObservableCollection<GoogleAutocompletePrediction>(rtn);
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
