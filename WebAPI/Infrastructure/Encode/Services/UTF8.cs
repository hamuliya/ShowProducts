using System.Text;
using WebAPI.Infrastructure.Encode.Interface;

namespace WebAPI.Infrastructure.Encode.Services
{
    public class UTF8 : IEncode
    {
        public byte[] Encode(string value)
        {
            return Encoding.UTF8.GetBytes(value);

        }
    }
}
