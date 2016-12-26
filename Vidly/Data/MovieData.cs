using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Data
{
    public class MovieData
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public short GenreId { get; set; }

        public GenreData Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime AddedDate { get; set; }

        [Range(1, 20)]
        public int NumberInStock { get; set; }
    }
}