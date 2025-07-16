using System;
using System.Security.Cryptography;

namespace Connect.Utilities
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
                throw new ArgumentNullException("password");

            using (var bytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(32);
            }

            byte[] dst = new byte[49];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer2, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;

            if (hashedPassword == null)
                return false;
            if (password == null)
                throw new ArgumentNullException("password");

            byte[] src = Convert.FromBase64String(hashedPassword);
            if (src.Length != 49 || src[0] != 0)
                return false;

            byte[] salt = new byte[16];
            Buffer.BlockCopy(src, 1, salt, 0, 16);

            byte[] storedSubkey = new byte[32];
            Buffer.BlockCopy(src, 17, storedSubkey, 0, 32);

            using (var bytes = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                buffer4 = bytes.GetBytes(32);
            }

            return ByteArraysEqual(storedSubkey, buffer4);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
