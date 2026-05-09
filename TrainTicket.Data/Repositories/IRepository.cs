using System.Linq.Expressions;

namespace TrainTicket.Data.Repositories
{
    // Generic repository cho các thao tác CRUD c? b?n b?ng EF Core.
    public interface IRepository<T> where T : class
    {
        // L?y t?t c? (dùng LINQ Where ?? l?c IsDeleted)
        IQueryable<T> GetAll();

        // L?y theo ID
        Task<T?> GetByIdAsync(int id);

        // Tìm theo ?i?u ki?n LINQ
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Thêm m?i
        Task AddAsync(T entity);

        // C?p nh?t
        void Update(T entity);

        // Xóa (th?t) — ít dùng, ch? y?u dùng soft delete
        void Delete(T entity);

        // L?u thay ??i vào DB
        Task SaveChangesAsync();
    }
}
