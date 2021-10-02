namespace BetFriend.UserAccess.Infrastructure.Hash
{
    using BetFriend.UserAccess.Domain;
    using System.Security.Cryptography;
    using System.Text;


    public sealed class Sha256HashPassword : IHashPassword
    {
        public string Hash(string password)
        {
            var data = Encoding.Default.GetBytes(password);
            var hhmac = new HMACSHA256(data);
            data = hhmac.ComputeHash(data);
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
    }
}
