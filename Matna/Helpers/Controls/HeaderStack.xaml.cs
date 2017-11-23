using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Matna.Helpers.Controls
{
    public partial class HeaderStack : StackLayout
    {
        public HeaderStack()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(HeaderStack), Color.White);
        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        public static readonly BindableProperty CenterIconProperty = BindableProperty.Create(nameof(CenterIcon), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty CenterTextProperty = BindableProperty.Create(nameof(CenterText), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty CenterCommandProperty =
            BindableProperty.Create(nameof(CenterCommand), typeof(ICommand), typeof(HeaderStack), null);
        public static readonly BindableProperty CenterVisibleProperty = BindableProperty.Create(nameof(CenterVisible), typeof(bool), typeof(HeaderStack), true);

        public static readonly BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIcon), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty LeftCommandProperty =
            BindableProperty.Create(nameof(LeftCommand), typeof(ICommand), typeof(HeaderStack), null);
        public static readonly BindableProperty LeftVisibleProperty = BindableProperty.Create(nameof(LeftVisible), typeof(bool), typeof(HeaderStack), true);

        public static readonly BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIcon), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(HeaderStack));
        public static readonly BindableProperty RightCommandProperty =
            BindableProperty.Create(nameof(RightCommand), typeof(ICommand), typeof(HeaderStack), null);
        public static readonly BindableProperty RightVisibleProperty = BindableProperty.Create(nameof(RightVisible), typeof(bool), typeof(HeaderStack), true);
        
        public string CenterIcon
        {
            get
            {
                return (string)GetValue(CenterIconProperty);
            }
            set
            {
                SetValue(CenterIconProperty, value);
            }
        }
        public string CenterText
        {
            get
            {
                return (string)GetValue(CenterTextProperty);
            }
            set
            {
                SetValue(CenterTextProperty, value);
            }
        }
        public ICommand CenterCommand
        {
            get { return (ICommand)GetValue(CenterCommandProperty); }
            set { SetValue(CenterCommandProperty, value); }
        }
        public bool CenterVisible
        {
            get
            {
                return (bool)GetValue(CenterVisibleProperty);
            }
            set
            {
                SetValue(CenterVisibleProperty, value);
            }
        }

        public string LeftIcon
        {
            get
            {
                return (string)GetValue(LeftIconProperty);
            }
            set
            {
                SetValue(LeftIconProperty, value);
            }
        }
        public string LeftText
        {
            get
            {
                return (string)GetValue(LeftTextProperty);
            }
            set
            {
                SetValue(LeftTextProperty, value);
            }
        }
        public ICommand LeftCommand
        {
            get { return (ICommand)GetValue(LeftCommandProperty); }
            set { SetValue(LeftCommandProperty, value); }
        }
        public bool LeftVisible
        {
            get
            {
                return (bool)GetValue(LeftVisibleProperty);
            }
            set
            {
                SetValue(LeftVisibleProperty, value);
            }
        }

        public string RightIcon
        {
            get
            {
                return (string)GetValue(RightIconProperty);
            }
            set
            {
                SetValue(RightIconProperty, value);
            }
        }
        public string RightText
        {
            get
            {
                return (string)GetValue(RightTextProperty);
            }
            set
            {
                SetValue(RightTextProperty, value);
            }
        }
        public ICommand RightCommand
        {
            get { return (ICommand)GetValue(RightCommandProperty); }
            set { SetValue(RightCommandProperty, value); }
        }
        public bool RightVisible
        {
            get
            {
                return (bool)GetValue(RightVisibleProperty);
            }
            set
            {
                SetValue(RightVisibleProperty, value);
            }
        }
    }
}