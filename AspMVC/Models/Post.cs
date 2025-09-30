using AspMVC.Scripts.Classes.Enums;

namespace AspMVC.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Description { get; set; }
        public string? MediaUrl { get; set; }
        public PublicEnums.MediaType MediaType { get; set; }
        public string? PubRuls { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
