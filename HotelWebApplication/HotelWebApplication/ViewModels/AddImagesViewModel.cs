using HotelWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.ViewModels
{
    /// <summary>
    /// Returns a list of images
    /// that were successfully added 
    /// to the system as wekk as errors
    /// </summary>
    public class AddImagesViewModel
    {
        public List<string> UploadErrors { get; set; }
        public List<Image> AddedImages { get; set; }
    }
}
