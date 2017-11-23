using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Matna.Helpers.Controls
{
    public partial class CardView : Grid
    {
        public CardView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty OnRouteClickedProperty =
            BindableProperty.Create(nameof(OnRouteClicked), typeof(ICommand), typeof(HeaderStack), null);

        public static readonly BindableProperty OnDetailClickedProperty =
            BindableProperty.Create(nameof(OnDetailClicked), typeof(ICommand), typeof(HeaderStack), null);

        public static readonly BindableProperty OnShareClickedProperty =
            BindableProperty.Create(nameof(OnShareClicked), typeof(ICommand), typeof(HeaderStack), null);

        public static readonly BindableProperty OnSaveClickedProperty =
            BindableProperty.Create(nameof(OnSaveClicked), typeof(ICommand), typeof(HeaderStack), null);

        public static readonly BindableProperty OnRemoveClickedProperty =
            BindableProperty.Create(nameof(OnRemoveClicked), typeof(ICommand), typeof(HeaderStack), null);
        
        public ICommand OnRouteClicked
        {
            get { return (ICommand)GetValue(OnRouteClickedProperty); }
            set { SetValue(OnRouteClickedProperty, value); }
        }

        public ICommand OnDetailClicked
        {
            get { return (ICommand)GetValue(OnDetailClickedProperty); }
            set { SetValue(OnDetailClickedProperty, value); }
        }

        public ICommand OnShareClicked
        {
            get { return (ICommand)GetValue(OnShareClickedProperty); }
            set { SetValue(OnShareClickedProperty, value); }
        }

        public ICommand OnSaveClicked
        {
            get { return (ICommand)GetValue(OnSaveClickedProperty); }
            set { SetValue(OnSaveClickedProperty, value); }
        }

        public ICommand OnRemoveClicked
        {
            get { return (ICommand)GetValue(OnRemoveClickedProperty); }
            set { SetValue(OnRemoveClickedProperty, value); }
        }
    }
}
