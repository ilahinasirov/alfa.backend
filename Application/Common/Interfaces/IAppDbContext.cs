using Domain.Constant;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        public DbSet<SystemConstant> SystemConstants { get; set; }
        Task AsTransactionAsync(Func<Task> func, CancellationToken cancellationToken = default);
        Task<T> AsTransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
