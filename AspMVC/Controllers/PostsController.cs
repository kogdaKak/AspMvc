using AspMVC.Models;
using AspMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspMVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostsRepository _repo;
        private readonly UploadMedia _uploader;
        private const int PageSize = 6;

        public PostsController(PostsRepository repo, UploadMedia uploader)
        {
            _repo = repo;
            _uploader = uploader;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetPaged(1,PageSize).ToList();
            ViewBag.PageSize = PageSize;
            return View(posts);
        }

        public IActionResult List(int page = 1)
        {
            var posts = _repo.GetPaged(page,PageSize).ToList();

            if(!posts.Any())
                return Content(string.Empty);

            return PartialView("_PostListPartail", posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile? mediaFile, string? description, string? pubLinks)
        {
            var (url, type) = await _uploader.SaveAsync(mediaFile);

            var post = new Post
            {
                Description = description,
                MediaUrl = url,
                MediaType = type,
                PubRuls = pubLinks,
                CreatedAt = DateTime.UtcNow
            };
            _repo.Add(post);

            return RedirectToAction(nameof(Index));
        }
    }
}
