using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieDatabase.Models.ViewModels
{
    public class RegisterEditMovieViewModel
    {
        [Required]
        public string MovieName { get; set; }

        [Required]
        public int? Rating { get; set; }

        [Required]
        public string Category { get; set; }
        
        [AllowHtml]
        public string Description { get; set; }
    }
}