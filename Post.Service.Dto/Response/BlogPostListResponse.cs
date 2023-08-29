using Newtonsoft.Json;
using Post.Service.DTO.FilterDto;

namespace Post.Service.DTO.Response
{
    public class BlogPostListResponse
    {
        [JsonProperty("blog_post_list")]
        public PagedViewResponse<BlogPostResponse> BlogPostList { get; set; }
    }

    public class BlogPostResponse
    {
        [JsonProperty("post_sid")]
        public string? BlogPostSid { get; set; }

        [JsonProperty("post_name")]
        public string? PostName { get; set; }

        [JsonProperty("post_description")]
        public string? PostDescription { get; set; }

        [JsonProperty("blog_image")]
        public string? BlogImage { get; set; }
    }
}
