using System.Linq.Expressions;

namespace MvcTask3.Repos
{
    public interface IRepository<T> where T : class
    {
        // ➕ إضافة عنصر جديد
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        // 🔁 تعديل عنصر
        void Update(T entity);

        // ❌ حذف عنصر
        void Delete(T entity);

        // 🔍 جلب مجموعة من العناصر (مع فلترة واختياري تضمين العلاقات)
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

        // 🔍 جلب عنصر واحد فقط
        Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

        // 💾 حفظ التغييرات
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
