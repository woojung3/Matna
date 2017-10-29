using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class FilterPageViewModel : BaseViewModel
    {
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
