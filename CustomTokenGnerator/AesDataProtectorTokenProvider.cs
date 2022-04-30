using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class AesDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
    private ILogger<DataProtectorTokenProvider<TUser>> logger;

    public AesDataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ISettingSupplier settingSupplier, ILogger<DataProtectorTokenProvider<TUser>> logger): base (dataProtectionProvider: new AesProtectionProvider(settingSupplier.Supply()), options, logger)
        {
            var settingsLifetime = settingSupplier.Supply().Encryption.PasswordResetLifetime;

            if (settingsLifetime.TotalSeconds > 1)
            {
                Options.TokenLifespan = TimeSpan.FromDays(1);
            }

        this.logger = logger;
    }
    }