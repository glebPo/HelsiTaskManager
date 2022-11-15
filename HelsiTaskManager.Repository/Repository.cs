using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoContext _context;
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IMongoQueryable<TEntity> _queryable;

        public IMongoContext Context => _context;

        public Repository(IMongoContext context)
        {
            _context = context;

            _collection = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            _queryable = _collection.AsQueryable<TEntity>();
        }
        public bool Any(Expression<Func<TEntity, bool>> where) => _queryable.Where(where).Any();

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where) => _queryable.Where(where).AnyAsync();

        public async Task<int> AddAsync(TEntity obj)
        {
            await _context.AddCommand(() => _collection.InsertOneAsync(obj));
            return await _context.SaveChanges();
        }

        public TEntity Get(ObjectId id) => _collection.Find(Builders<TEntity>.Filter.Eq("_id", id)).SingleOrDefault();

        public async Task<TEntity> GetAsync(ObjectId id) {
            var collection = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return await collection.SingleOrDefaultAsync();
        }

        public virtual async Task<int> Update(TEntity obj)
        {
            await _context.AddCommand(() => _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj));
            return await _context.SaveChanges();
        }

        public virtual async Task<int> RemoveAsync(ObjectId id)
        {
            await _context.AddCommand(() => _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
            return await _context.SaveChanges();
        }
        
        public void Dispose() =>
            _context?.Dispose();
    }
}
