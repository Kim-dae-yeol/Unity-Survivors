namespace Model.Player
{
    public class UserInfo
    {
        public string Name { get; }
        public int Level;
        public int Exp;
        public string ProfileName;

        public UserInfo(string name)
        {
            Name = name;
        }
    }
}