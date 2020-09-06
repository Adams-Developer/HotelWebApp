using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomType RoomType { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "bit")]
        public bool Available { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "varchar(max)")]
        public string Description { get; set; }

        [Required]
        public int MaximumGuests { get; set; }

        public virtual ICollection<RoomFeature> Features { get; set; }

        public virtual ICollection<Image> RoomImages { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
