using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Data.DbContexts;

namespace TrainTicket.Data.Repositories
{
    // Cài ??t repository dùng DbSet<T> c?a EF Core.
    // Dùng cho các module CRUD danh m?c.
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly TrainTicketDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(TrainTicketDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll() => _dbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
