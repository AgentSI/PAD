using System.ComponentModel.DataAnnotations;

namespace PostMaker.Models
{
    public class CreatePostModel
    {
        [Required]
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }
    }
}