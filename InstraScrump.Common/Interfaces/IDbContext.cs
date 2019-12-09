using System.Threading.Tasks;

namespace InstaScrump.Common.Interfaces
{
    public interface IDbContext<out T>
    {
        T Create(bool test = false);
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}