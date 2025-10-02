using AspMVC.Data;         // подключаем DbContext
using AspMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AspMVC.Services
{
    /// <summary>
    /// Репозиторий постов. Работает через AppDbContext и БД Postgres.
    /// </summary>
    public class PostsRepository
    {
        private readonly AppDbContext _context;

        // DbContext внедряется через конструктор
        public PostsRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавить пост в БД.
        /// Обязательно вызываем SaveChangesAsync, чтобы запись сохранилась.
        /// </summary>
        public async Task AddAsync(Post post)
        {
            _context.Posts.Add(post);                  // добавляем объект в EF-трекер
            await _context.SaveChangesAsync();         // фиксируем изменения в БД
        }

        /// <summary>
        /// Получить посты с пагинацией (страницы по PageSize).
        /// Сортируем по CreatedAt по убыванию (новые сверху).
        /// </summary>
        public async Task<List<Post>> GetPagedAsync(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            return await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Получить количество всех постов.
        /// </summary>
        public async Task<int> CountAsync()
        {
            return await _context.Posts.CountAsync();
        }
    }
}
