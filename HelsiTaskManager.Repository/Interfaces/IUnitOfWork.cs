using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        #region
        public IRepository<TaskList> TaskList { get; }
        public IRepository<User> User { get; }
        #endregion
        Task<bool> Commit();
    }
}
