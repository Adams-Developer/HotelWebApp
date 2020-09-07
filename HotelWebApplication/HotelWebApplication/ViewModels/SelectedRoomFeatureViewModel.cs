using HotelWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.ViewModels
{
    public class SelectedRoomFeatureViewModel
    {
        public int FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public bool Selected { get; set; }
    }
}
