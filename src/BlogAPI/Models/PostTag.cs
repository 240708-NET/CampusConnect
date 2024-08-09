using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

[ExcludeFromCodeCoverage]
public class PostTag : IIdentified
{
    public int ID { get; set; }
    [Required]
    public int PostID { get; set; }
    [Required]
    public int TagID { get; set; }

    [JsonIgnore]
    public Post Post { get; set; } = null!;
    [JsonIgnore]
    public Tag Tag { get; set; } = null!;
}