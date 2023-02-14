namespace WebAPI.Infrastructure.Encode.Interface
{
    public interface IEncode
    {
        byte[] Encode(string value);
    }
}