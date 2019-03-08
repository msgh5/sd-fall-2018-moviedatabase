using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDatabase.Models.ViewModels
{
    public class DetailsMovieViewModel
    {
        public string MovieName { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
    }
}