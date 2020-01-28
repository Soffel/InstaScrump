using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using InstaScrump.Common.Interfaces;

namespace InstaScrump.Common.Utils
{
    public class Config : IConfig
    {
        private readonly string _path;
        private readonly string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);

        public Config(string iniPath = null)
        {
            _path = new FileInfo(iniPath ?? _exe + ".ini").FullName;
            WriteDefaultIniFile();
        }

        public string Read(string key, string section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _exe, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void Write(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _exe, key, value, _path);
        }

        public void DeleteKey(string key, string section = null)
        {
            Write(key, null, section ?? _exe);
        }

        public void DeleteSection(string section = null)
        {
            Write(null, null, section ?? _exe);
        }

        public bool KeyExists(string key, string section = null)
        {
            return Read(key, section).Length > 0;
        }

        public void WriteDefaultIniFile()
        {
            if (!KeyExists("Download", "Path"))
            {
                Write("Download", @"Path\To\Saved\Pictures", "Path");
            }

            if (!KeyExists("Log", "Path"))
            {
                Write("Log", @"Path\To\LogFiles", "Path");
            }

            if (!KeyExists("Csv", "Path"))
            {
                Write("Csv", @"Path\For\CSV\Import", "Path");
            }

            if (!KeyExists("SaveLoginFile", "Path"))
            {
                Write("SaveLoginPath",  @"Path\For\Save\File", "Data");
            }

            if (!KeyExists("Separator"))
            {
                Write("Separator", @"<;>");
            }

            if (!KeyExists("CsvSeparator"))
            {
                Write("CsvSeparator", @";");
            }

            if (!KeyExists("RequestLimitPerHour", "Config"))
            {
                Write("RequestLimit", @"2500", "Config");
            }

            if (!KeyExists("RequestLimitPerMinute", "Config"))
            {
                Write("RequestLimit", @"50", "Config");
            }

            if (!KeyExists("MaxLoginAttempt", "Config"))
            {
                Write("MaxLoginAttempt", @"5", "Config");
            }

            if (!KeyExists("Vector", "Security"))
            {
                Write("Vector", KeyGenerator.GetUniqueKey(16), "Security");
            }

            if (!KeyExists("Pswd", "Security"))
            {
                Write("Pswd", KeyGenerator.GetUniqueKey(35), "Security");
            }

            if (!KeyExists("Prod", "Database"))
            {
                Write("Prod", @"DataSource=G:\Projekte\Database\InstaScrump.db;", "Database");
            }

            if (!KeyExists("Test", "Database"))
            {
                Write("Test", @"DataSource=G:\Projekte\Database\Test\InstaScrump.db;", "Database");
            }

        }
    }
}
