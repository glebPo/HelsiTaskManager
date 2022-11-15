using System.Linq.Expressions;

namespace HelsiTaskManager.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        public IMongoContext Context { get; }
        Task<int> AddAsync(TEntity obj);
        Task<int> Update(TEntity obj);
        Task<int> RemoveAsync(ObjectId id);
        bool Any(Expression<Func<TEntity, bool>> where);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
        TEntity Get(ObjectId id);
        Task<TEntity> GetAsync(ObjectId id);
    }
}
