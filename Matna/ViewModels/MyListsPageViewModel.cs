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
            OnRemoveClicked = new Command(async (arg) => {
                try
                {
                    await PropertiesDictionary.SavedLists.Single(s => s.Name == arg.ToString()).RemoveFromFileAsync();
                    PropertiesDictionary.SavedLists.Remove(PropertiesDictionary.SavedLists.Single(s => s.Name == arg.ToString()));
                }
                catch (Exception ex) { } // Do nothing. Not exists

                MessagingCenter.Send(this, "HideMyLists");
            });
        }

        public ICommand OnCloseClicked { get; }
        public ICommand OnRemoveClicked { get; }
    }
}
