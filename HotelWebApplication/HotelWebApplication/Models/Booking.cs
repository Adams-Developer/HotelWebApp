using HotelWebApplication.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        [Column(TypeName = "varchar(50)")]
        public virtual Room Room { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CheckIn { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CheckOut { get; set; }

        [Column(TypeName = "varchar(max)")]
        public int Guests { get; set; }

        [Column(TypeName = "varchar(50)")]
        public decimal TotalFee { get; set; }

        [Column(TypeName = "bit")]
        public bool Paid { get; set; }

        [Column(TypeName = "bit")]
        public bool Completed { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerFirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerLastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string CustomerEmail { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerPhone { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerCity { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerState { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CustomerZipCode { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string OtherRequests { get; set; }

    }
}
