using BetFriend.UserAccess.Domain;
using System.Security.Cryptography;
using System.Text;

namespace BetFriend.UserAccess.Infrastructure.Hash
{
    public class MD5HashPassword : IHashPassword
    {
        public string Hash(string password)
        {
            var data = Encoding.Default.GetBytes(password);
            var hhmac = new HMACMD5(data);
            data = hhmac.ComputeHash(data);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
    }
}
