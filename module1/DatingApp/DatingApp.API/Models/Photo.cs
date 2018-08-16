using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DatedAdded { get; set; }
        public bool IsMain { get; set; }

        /* Relationship adding this will automatically set the table as cascade delete*/
        public User user { get; set; }
        public int UserId { get; set; }
    }
}