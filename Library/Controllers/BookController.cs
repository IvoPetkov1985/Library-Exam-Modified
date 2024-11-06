using Library.Contracts;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using static Library.Data.Common.DataConstants;

namespace Library.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService service;

        public BookController(IBookService bookService)
        {
            service = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await service.GetAllBooksAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new BookFormModel()
            {
                Categories = await service.GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookFormModel model)
        {
            var categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), MissingCategoryErrorMsg);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
            }

            await service.CreateNewEntityAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            await service.AddBookToCollectionAsync(userId, id);

            return RedirectToAction(nameof(Mine));
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = GetUserId();

            var model = await service.GetMineBooksAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            await service.RemoveBookFromCollectionAsync(userId, id);

            return RedirectToAction(nameof(Mine));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            var model = await service.GetEditBookModelAsync(id);

            model.Categories = await service.GetCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookFormModel model, int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            var categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), MissingCategoryErrorMsg);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }

            await service.EditBookAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            await service.DeleteBookAsync(id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool isBookExisting = await service.IsBookExistingAsync(id);

            if (isBookExisting == false)
            {
                return NotFound();
            }

            var model = await service.CreateDetailsModelAsync(id);

            return View(model);
        }
    }
}
