using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDatabase.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
    }
}