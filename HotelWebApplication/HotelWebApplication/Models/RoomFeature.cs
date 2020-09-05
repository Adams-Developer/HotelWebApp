using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class RoomFeature
    {
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public int FeatureId { get; set; }

        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
    }
}
