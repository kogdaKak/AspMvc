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

        public async Task<IActionResult> Index()
        {
            var posts = await _repo.GetPagedAsync(1, PageSize);
            ViewBag.PageSize = PageSize;
            return View(posts);
        }

        public async Task<IActionResult> List(int page = 1)
        {
            var posts = await _repo.GetPagedAsync(page, PageSize);

            if (!posts.Any())
                return Content(string.Empty);

            return PartialView("_PostListPartial", posts);

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
            await _repo.AddAsync(post);

            return RedirectToAction(nameof(Index));
        }
    }
}
