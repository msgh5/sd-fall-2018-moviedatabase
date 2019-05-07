using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDatabase.Models.Domain
{
    public class MovieHistory
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Property { get; set; }

        public Movie Movie { get; set; }
        public int MovieId { get; set; }
    }
}