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
    public class FilterPageViewModel : BaseViewModel
    {
        public int MaxRadKM
        {
            get
            {
                return (int)PropertiesDictionary.MaxRadKM;
            }
            set
            {
                PropertiesDictionary.MaxRadKM = value;
                OnPropertyChanged();
            }
        }

        public int GoogleRecIdx
        {
            get
            {
                return (int)PropertiesDictionary.GoogleRecIdx;
            }
            set
            {
                PropertiesDictionary.GoogleRecIdx = value;
                OnPropertyChanged();
            }
        }

        public bool ShowGoogle
        {
            get
            {
                return PropertiesDictionary.ShowGoogle;
            }
            set
            {
                PropertiesDictionary.ShowGoogle = value;
                OnPropertyChanged();
            }
        }

        public string Keyword
        {
            get
            {
                return PropertiesDictionary.Keyword;
            }
            set
            {
                PropertiesDictionary.Keyword = value;
                OnPropertyChanged();
            }
        }

        public FilterPageViewModel()
        {
            // Properties (Commands) initilization
            OnCloseClicked = new Command(() => {
                MessagingCenter.Send(this, "HideFilter");
            });
        }

        public ICommand OnCloseClicked { get; }
    }
}
