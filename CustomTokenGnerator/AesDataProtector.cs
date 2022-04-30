using System.Text;
using Encryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

public class AesDataProtector : IDataProtector
    {
        private readonly string _purpose;
        private readonly SymmetricSecurityKey _key;
        private readonly Encoding _encoding = Encoding.UTF8;

        public AesDataProtector(string purpose, string key)
        {
            _purpose = purpose;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public byte[] Protect(byte[] userData)
        {
            return AESThenHMAC.SimpleEncryptWithPassword(userData, _encoding.GetString(_key.Key));
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            var restlu = AESThenHMAC.SimpleDecryptWithPassword(protectedData, _encoding.GetString(_key.Key));
            return restlu;
        }

        public IDataProtector CreateProtector(string purpose)
        {
            throw new NotSupportedException();
        }
    }