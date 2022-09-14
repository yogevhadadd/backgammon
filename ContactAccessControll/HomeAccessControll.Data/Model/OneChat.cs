namespace HomeAccessControll.Data.Model
{
    public class OneChat
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FirstPic { get; set; }
        public string? SecondPic { get; set; }
        public string? FirstUserName { get; set; }
        public string? SecondUserName { get; set; }
        public List<string>? Chat { get; set; }
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
