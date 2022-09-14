using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeAccessControll.Data.Model
{
    [Table("User")]
    public partial class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(255)]
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = null!;
        [StringLength(255)]
        public string Passwors { get; set; } = null!;
        [StringLength(255)]
        public string? DisplayName { get; set; }
        [StringLength(255)]
        public string? ProfilePic { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiryTime { get; set; } = null;

    }
}
