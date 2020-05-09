namespace InstaScrump.Common.Interfaces
{
    public interface IConfig
    {
        string Read(string key, string section = null);
        T Read<T>(string key, string section = null);
        void Write(string key, string value, string section = null);
    }
}
