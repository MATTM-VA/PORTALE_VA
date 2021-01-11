using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VALib.Domain.Services
{
    internal class PasswordHasher
    {
        private const int _saltByteSize = 32;
        private const int _hashSize = 32;
        private const int _iterations = 1100;

        internal PasswordHasher()
        {

        }

        internal string HashPassword(string password)
        {
            string result = null;
            RNGCryptoServiceProvider rngCsp = null;
            byte[] saltBytes = null;
            byte[] hashBytes = null;

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Argument is null or white space", "password");

            // SALT
            saltBytes = new byte[_saltByteSize];
            rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(saltBytes);
            rngCsp.Dispose();
            rngCsp = null;

            // HASH
            hashBytes = this.Pbkdf2Hash(password, saltBytes, _iterations);
           

            result = _iterations.ToString(CultureInfo.InvariantCulture) + ":" +
                      Convert.ToBase64String(saltBytes) + ":" +
                      Convert.ToBase64String(hashBytes);
            
            return result;
        }

        internal bool ValidatePassword(string password, string expectedHash)
        {
            bool isValid = false;
            byte[] saltBytes = null;
            int iterations = 0;
            byte[] expectedHashBytes = null;
            byte[] hashBytes = null;

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Argument is null or white space", "password");

            if (string.IsNullOrWhiteSpace(expectedHash))
                throw new ArgumentException("Argument is null or white space", "expectedHash");

            try
            {
                string[] expectedHashTokens = expectedHash.Split(':');

                iterations = int.Parse(expectedHashTokens[0], CultureInfo.InvariantCulture);
                saltBytes = Convert.FromBase64String(expectedHashTokens[1]);
                expectedHashBytes = Convert.FromBase64String(expectedHashTokens[2]);
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid format for expectedHash argument.", ex);
            }

            hashBytes = this.Pbkdf2Hash(password, saltBytes, iterations);

            isValid = this.SlowEquals(hashBytes, expectedHashBytes);

            return isValid;
        }

        private byte[] Pbkdf2Hash(string password, byte[] saltBytes, int iterations)
        {
            byte[] result = null;
            Rfc2898DeriveBytes pbkdf2 = null;

            pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, iterations);
            result = pbkdf2.GetBytes(_hashSize);
            pbkdf2.Dispose();

            return result;
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);

            return diff == 0;
        }
    }
}
