using System.Text.Json.Serialization;

namespace Chat.Model
{
    public class OneChat
    {
        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("SecondName")]
        public string? SecondName { get; set; }
        [JsonPropertyName("FirstPic")]
        public string? FirstPic { get; set; }
        [JsonPropertyName("SecondPic")]
        public string? SecondPic { get; set; }
        [JsonPropertyName("FirstUserName")]
        public string? FirstUserName { get; set; }
        [JsonPropertyName("SecondUserName")]
        public string? SecondUserName { get; set; }
        [JsonIgnore]
        public List<ChatMessage>? Chat { get; set; } = new List<ChatMessage>();
        public override bool Equals(object? obj)
        {
            OneChat other = obj as OneChat;
            return other?.FirstName == FirstName && other?.SecondName == SecondName;
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
