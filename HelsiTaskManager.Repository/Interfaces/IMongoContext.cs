using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace HelsiTaskManager.Repository
{
    public interface IMongoContext : IDisposable
    {
        public IMongoDatabase Database { get; }

        Task AddCommand(Func<Task> func);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
