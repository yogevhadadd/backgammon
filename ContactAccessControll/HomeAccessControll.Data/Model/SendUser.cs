
namespace HomeAccessControll.Data.Model
{
    public class SendUser
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfilePic { get; set; }
        public string? FirstPic { get; set; }
        public string? SecondPic { get; set; }
        public bool Online { get; set; }

        public override bool Equals(object? obj)
        {
            SendUser other = obj as SendUser;
            return other?.DisplayName == DisplayName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
