using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;

namespace InstaScrump.Database.Model
{
    public static partial class TableExtensions
    {
        public static bool IsBetween<T>(this T x, T low, T high) where T : IComparable<T>
        {
            return x.CompareTo(low) >= 0 && x.CompareTo(high) <= 0;
        }

        public static bool IsBiggerThan<T>(this T x, T low) where T : IComparable<T>
        {
            return x.CompareTo(low) >= 0;
        }

        public static bool IsSmallerThan<T>(this T x, T high) where T : IComparable<T>
        {
            return x.CompareTo(high) <= 0;
        }

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
    }
}