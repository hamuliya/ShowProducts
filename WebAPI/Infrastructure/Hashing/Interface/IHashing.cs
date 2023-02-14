namespace WebAPI.Infrastructure.Hashing.Interface
{
    public interface IHashing
    {
        string GeneratePasswordHash(string password);
        string GenerateSalt(int length);
        bool Verify(string password, string passwordHash);
    }
}