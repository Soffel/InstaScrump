using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;

namespace InstaScrump.Database.Model
{
    public static partial class TableExtensions
    {
        public static LoginData Find(this ITable<LoginData> table, string userName)
        {
            return table.FirstOrDefault(t =>
                t.UserName.Equals(userName));
        }
        public static async Task<LoginData> FindAsync(this ITable<LoginData> table, string userName)
        {
            return await table.FirstOrDefaultAsync(t =>
                t.UserName.Equals(userName));
        }

        public static Follow Find(this ITable<Follow> table, string userName)
        {
            return table.FirstOrDefault(t =>
                t.UserName.Equals(userName));
        }
        public static async Task<Follow> FindAsync(this ITable<Follow> table, string userName)
        {
            return await table.FirstOrDefaultAsync(t =>
                t.UserName.Equals(userName));
        }
    }
}