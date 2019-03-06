using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDatabase.Models.ViewModels
{
    public class IndexMovieViewModel
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
    }
}