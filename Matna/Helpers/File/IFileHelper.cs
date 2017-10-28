using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Matna.Models;
using Xamarin.Forms;

namespace Matna.Helpers.File
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
