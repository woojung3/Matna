using System;
using Xamarin.Forms;

namespace Matna.Helpers.Controls
{
    public class FontAwesomeIcon : Label
    {
        //Must match the exact "Name" of the font which you can get by double clicking the TTF in Windows
        public const string Typeface = "FontAwesome";

        //Constructor for XAML use
        //<local:FontAwesomeIcon Text="{x:Static local:FontAwesomeIcon.Icon.Gear}"></local:FontAwesomeIcon>
        public FontAwesomeIcon()
        {
            FontFamily = Typeface;
        }

        public FontAwesomeIcon(string fontAwesomeIcon = null)
        {
            FontFamily = Typeface;    //iOS is happy with this, Android needs a renderer to add ".ttf"
            Text = fontAwesomeIcon;
        }
    }

    /// <summary>
    /// Get more icons from http://fortawesome.github.io/Font-Awesome/cheatsheet/
    /// Tip: Just copy and past the icon picture here to get the icon
    /// 
    /// https://github.com/neilkennedy/FontAwesome.Xamarin/blob/master/FontAwesome.Xamarin/FontAwesome.cs
    /// For a huge list of icon codes
    /// </summary>
    public static class Icon
    {
        public static readonly string Search = "\uf002";
        public static readonly string AngleDoubleUp = "\uf102";
        public static readonly string AngleDoubleDown = "\uf103";
        public static readonly string Sliders = "\uf1de";
        public static readonly string StepBackward = "\uf048";
        public static readonly string Start = "\uf005";
        public static readonly string StartHalfO = "\uf123";
        public static readonly string Dollar = "\uf155";
        public static readonly string Google = "\uf1a0";
        public static readonly string MapO = "\uf278";
        public static readonly string MapMarker = "\uf041";
        public static readonly string FolderOpenO = "\uf115";
    }
}