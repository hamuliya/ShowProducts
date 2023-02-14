using WebAPI.Infrastructure.Hashing.Interface;

namespace WebAPI.Infrastructure.Hashing.Services;

public class BCryptHashing : IHashing
{

    public string GeneratePasswordHash(string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        return passwordHash;
    }
    public string GenerateSalt(int length)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(length);
        return salt;
    }

    public bool Verify(string password, string passwordHash)
    {
        bool verified = BCrypt.Net.BCrypt.Verify(password, passwordHash);
        return verified;
    }
}
