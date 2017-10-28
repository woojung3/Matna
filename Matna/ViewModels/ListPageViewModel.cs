using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class ListPageViewModel : BaseViewModel
    {
        DateTime dateTo = DateTime.Today.AddMonths(1);
        public DateTime DateTo
        {
            get
            {
                return dateTo;
            }
            set
            {
                dateTo = value;
                OnPropertyChanged("TourLists");
            }
        }

        public ListPageViewModel()
        {
            // Properties (Commands) initilization
            OnListClicked = new Command(() => {
                MessagingCenter.Send(this, "HideList");
            });
            OnLoadClicked = new Command(() => {
                MessagingCenter.Send(this, "DisplayAlert", "Load");
            });
        }

        public ICommand OnSearchClicked { get; }
        public ICommand OnFilterClicked { get; }
        public ICommand OnListClicked { get; }
        public ICommand OnLoadClicked { get; }
    }
}
