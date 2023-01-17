namespace WebAPI.Global.Encode
{
    public class UTF8 : IEncode
    {
        public byte[] Encode(string value = "I love coding")
        {
            return System.Text.Encoding.UTF8.GetBytes(value);
        }
    }
}
