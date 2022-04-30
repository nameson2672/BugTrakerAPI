

using Microsoft.AspNetCore.DataProtection;

public class AesProtectionProvider : IDataProtectionProvider
    {
        private readonly SystemSettings _settings;

        public AesProtectionProvider(SystemSettings settings)
        {
            _settings = settings;

            if(string.IsNullOrEmpty(_settings.Encryption.AESPasswordResetKey))
                throw new ArgumentNullException("AESPasswordResetKey must be set");
        }

        public IDataProtector CreateProtector(string purpose)
        {
            return new AesDataProtector(purpose, _settings.Encryption.AESPasswordResetKey);
        }
    }