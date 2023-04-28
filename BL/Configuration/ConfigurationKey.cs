using Microsoft.Extensions.Configuration;

namespace BL.Configuration
{
    public class ConfigurationKey : IConfigurationKey
    {
        private IConfiguration configuration;

        public ConfigurationKey(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string EncryptKey => configuration["Key:EncryptKey"];

        public string EncrpytByte => configuration["Key:EncryptByte"];

        public string EmailLogin => configuration["Key:EmailLogin"];

        public string EmailPassword => configuration["Key:EmailPassword"];
    }
}
