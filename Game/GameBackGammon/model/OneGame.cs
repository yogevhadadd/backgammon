using Game.Service;
using System.Text.Json.Serialization;

namespace GameBackGammon.model
{
    public class OneGame
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
        public GameService? GameService { get; set; } = new GameService();
        public override bool Equals(object? obj)
        {
            OneGame other = obj as OneGame;
            return other?.FirstName == FirstName && other?.SecondName == SecondName;
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
