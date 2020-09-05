using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class RoomType
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
