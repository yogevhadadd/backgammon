namespace TalkBackAccessControll.Date.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
