using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chat.Model
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        [StringLength(255)]
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = null!;
        [StringLength(255)]
        public string Passwors { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [StringLength(255)]
        public string? Token { get; set; }
        [StringLength(255)]
        public string? DisplayName { get; set; }
        public int? Rating { get; set; }
        [StringLength(255)]
        public string? ProfilePic { get; set; }
    }
}
