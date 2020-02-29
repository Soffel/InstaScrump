using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using InstaScrump.Common.Interfaces;
using InstaScrump.Common.Constants;
using System;

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
            if (!KeyExists(ConfigKey.Download_Key, "Path"))
            {
                Write(ConfigKey.Download_Key, @"Path\To\Saved\Pictures", "Path");
            }

            if (!KeyExists(ConfigKey.Log_Key, "Path"))
            {
                Write(ConfigKey.Log_Key, @"Path\To\LogFiles", "Path");
            }

            if (!KeyExists(ConfigKey.Csv_Key, "Path"))
            {
                Write(ConfigKey.Csv_Key, @"Path\For\CSV\Import", "Path");
            }

            if (!KeyExists(ConfigKey.Login_File_Key, "Path"))
            {
                Write(ConfigKey.Login_File_Key,  @"Path\For\Save\File", "Data");
            }

            if (!KeyExists(ConfigKey.Separator_Key))
            {
                Write(ConfigKey.Separator_Key, @"<;>");
            }

            if (!KeyExists(ConfigKey.Csv_Separator_Key))
            {
                Write(ConfigKey.Csv_Separator_Key, @";");
            }

            if (!KeyExists(ConfigKey.Request_Limit_Per_Hour_Key, "Config"))
            {
                Write(ConfigKey.Request_Limit_Per_Hour_Key, @"2500", "Config");
            }

            if (!KeyExists(ConfigKey.Request_Limit_Per_Minute_Key, "Config"))
            {
                Write(ConfigKey.Request_Limit_Per_Minute_Key, @"50", "Config");
            }

            if (!KeyExists(ConfigKey.Max_Login_Key, "Config"))
            {
                Write(ConfigKey.Max_Login_Key, @"5", "Config");
            }

            if (!KeyExists(ConfigKey.Vector_Key, "Security"))
            {
                Write(ConfigKey.Vector_Key, KeyGenerator.GetUniqueKey(16), "Security");
            }

            if (!KeyExists(ConfigKey.Pswd_Key, "Security"))
            {
                Write(ConfigKey.Pswd_Key, KeyGenerator.GetUniqueKey(35), "Security");
            }

            if (!KeyExists(ConfigKey.Prod_Db_Key, "Database"))
            {
                Write(ConfigKey.Prod_Db_Key, @"DataSource=G:\Projekte\Database\InstaScrump.db;", "Database");
            }

            if (!KeyExists(ConfigKey.Test_Db_Key, "Database"))
            {
                Write(ConfigKey.Test_Db_Key, @"DataSource=G:\Projekte\Database\Test\InstaScrump.db;", "Database");
            }

        }

        public T Read<T>(string key, string section = null)
        {
           return (T)Convert.ChangeType(Read(key, section), typeof(T));
        }
    }
}
