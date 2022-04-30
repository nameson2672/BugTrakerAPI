public interface ISettingSupplier
    {
        SystemSettings Supply();
    }

    public class SettingSupplier : ISettingSupplier
    {
        private IConfiguration Configuration { get; }

        public SettingSupplier(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public SystemSettings Supply()
        {
            var settings = new SystemSettings();
            Configuration.Bind("SystemSettings", settings);

            return settings;
        }
    }

    public class SystemSettings
    {
        public EncryptionSettings Encryption { get; set; } = new EncryptionSettings();
    }

    public class EncryptionSettings
    {
        public string AESPasswordResetKey { get; set; } = "hellowsdmfbsdkfhbjhdfkjdhfkjd";
        public TimeSpan PasswordResetLifetime { get; set; } = new TimeSpan(1,0,0,0,0);
    }