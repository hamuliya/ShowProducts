namespace WebAPI.Global.Encode
{
    public class UTF8 : IEncode
    {
        public byte[] Encode(string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);
        }
    }
}
