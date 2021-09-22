namespace BetFriend.UserAccess.Domain
{
    public interface IHashPassword
    {
        string Hash(string password);
    }
}
