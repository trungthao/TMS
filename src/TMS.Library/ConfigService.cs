using TMS.Library.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TMS.Library
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfiguration _appSettings;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = _configuration.GetSection("AppSettings");
        }
        public string GetAppSettingByKey(string key)
        {
            return _appSettings[key];
        }

        public string GetConnectionStrings()
        {
            return _configuration.GetConnectionString("MysqlConnectionString");
        }
    }
}