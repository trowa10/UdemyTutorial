using System;

namespace DatingApp.API.Models
{
        // Note every time you update the model you should create new migration
        //dotnet ef migrations add yourdescriptions
        // after dotnet ef database update
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DatedAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicID { get; set; }
        /* Relationship adding this will automatically set the table as cascade delete*/
        public User user { get; set; }
        public int UserId { get; set; }
    }
}