using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Post.Service.DTO.Request
{
    public class BlogPostRequestModel
    {
        [Required]
        [MaxLength(50)]
        [JsonProperty("post_name")]

        public string? PostName { get; set; }
        [JsonProperty("post_description")]

        [Required]
        [MaxLength(500)]
        public string? PostDescription { get; set; }
        [JsonProperty("file")]

        [Required]
        public IFormFile? File { get; set; }

    }

    
}
