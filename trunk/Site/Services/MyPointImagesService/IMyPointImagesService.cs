using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Services.MyPointImagesService
{
    public interface IMyPointImagesService
    {
        string[] GetMyPointImages(string[] descriptions);
    }
}
