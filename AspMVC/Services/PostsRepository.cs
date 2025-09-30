using AspMVC.Models;
using AspMVC.Scripts.Classes.Enums;

namespace AspMVC.Services
{
    public class PostsRepository
    {
        private readonly List<Post> _posts = new List<Post>();

        private int _nextId = 1;

        public PostsRepository()
        {
            _posts.Add(new Post
            {
                Id = _nextId++,
                Description = "Тестовый пост без медиа",
                MediaUrl = null,
                MediaType = PublicEnums.MediaType.None,
                PubRuls = "https://github.com/",
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            });

            _posts.Add(new Post
            {
                Id = _nextId++,
                Description = "Тестовый пост 2",
                MediaUrl = null,
                MediaType = PublicEnums.MediaType.None,
                PubRuls = "https://toutube.com/",
                CreatedAt = DateTime.UtcNow
            });
        }

        public void Add(Post post)
        {
            post.Id = _nextId++;
            _posts.Add(post);
        }

        public IEnumerable<Post> GetPaged(int page, int pageSize)
        {
            if (page < 1)
                page = 1;
            return _posts
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int Count => _posts.Count;
    }
}
