using System.Text.Json.Serialization;

namespace TalkBackAccessControll.Date.Models
{
    public class Token
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? NickName { get; set; }
    }
}