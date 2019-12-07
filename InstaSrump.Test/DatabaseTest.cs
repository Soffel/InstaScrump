using System.Threading.Tasks;
using InstaScrump.Database.Model;
using LinqToDB;
using Xunit;

namespace InstaSrump.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task ConnectionTest()
        {
            using (var db = new DbContext().Create(true))
            {
                Assert.NotNull(db);
                Assert.IsType<InstaScrumpDB>(db);

                await db.Follows.DeleteAsync();

                Assert.True(await db.Follows.InsertAsync(() => new Follow
                    {Favorit = true, FullName = "Maxi Mustermensch", InstaPk = 1, Username = "MaMu"}) == 1);
            }
        }
    }
}
