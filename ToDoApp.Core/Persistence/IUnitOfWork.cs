using System;
using System.Threading.Tasks;

namespace ToDoApp.Core.Persistence
{
    public interface IUnitOfWork<out T> : IDisposable
    {
        T DatabaseContext { get; }
        void Commit();
        Task CommitAsync();
    }
}
