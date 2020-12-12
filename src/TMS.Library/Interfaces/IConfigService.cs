namespace TMS.Library.Interfaces
{
    public interface IConfigService
    {
        string GetAppSettingByKey(string key);

        string GetConnectionStrings();
    }
}