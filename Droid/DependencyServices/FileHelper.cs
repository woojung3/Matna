using System;
using System.IO;
using Matna.Droid;
using Matna.Helpers.File;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Matna.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}