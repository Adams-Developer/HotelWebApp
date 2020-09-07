using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    // Many to Many relationship between Room and Feature entities
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
