using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required] //adding data annotation will require updating of database. make sure to create migration first
        public string Name { get; set; }
    }
}
