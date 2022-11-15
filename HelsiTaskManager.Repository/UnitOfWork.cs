namespace HelsiTaskManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        #region repos

        public IRepository<TaskList> TaskList => Repository<TaskList>();

        public IRepository<User> User => Repository<User>();
        #endregion

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new() =>
            _repositories.TryGetValue(typeof(TEntity), out object repository)
                ? (IRepository<TEntity>)repository
                : (IRepository<TEntity>)(_repositories[typeof(TEntity)] = new Repository<TEntity>(_context));

        public async Task<bool> Commit()
        {
            return await _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
