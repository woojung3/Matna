using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Matna.Helpers;
using Matna.Models;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class MyListsPageViewModel : BaseViewModel
    {
        public List<GooglePlaceNearbyList> SavedLists
        {
            get
            {
                return PropertiesDictionary.SavedLists;
            }
        }

        public MyListsPageViewModel()
        {
            // Properties (Commands) initilization
            OnCloseClicked = new Command(() => {
                MessagingCenter.Send(this, "HideMyLists");
            });
        }

        public ICommand OnCloseClicked { get; }
    }
}
