using System.Text.Json.Serialization;

namespace WebBlog.Models
{
    public class Category
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<Post>? Posts { get; set; } = new List<Post>();
    }
}