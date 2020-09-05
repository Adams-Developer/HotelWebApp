using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApplication.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public string ImageUrl { get; set; }

        public string FilePath { get; set; }
    }
}
