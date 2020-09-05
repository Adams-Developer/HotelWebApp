using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public string ReviewerName { get; set; }

        public string ReviewerEmail { get; set; }

        [Column(TypeName = "varchar(max)")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
