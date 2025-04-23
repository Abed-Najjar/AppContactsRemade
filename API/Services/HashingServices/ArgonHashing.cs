using Isopoh.Cryptography.Argon2;

namespace API.Services.HashingServices
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return Argon2.Hash(password);
        }

        public static bool VerifyPassword(string hash, string password)
        {
            return Argon2.Verify(hash, password);
        }
    }
}
