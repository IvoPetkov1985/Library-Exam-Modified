using Library.Contracts;
using Library.Data;
using Library.Data.DataModels;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task AddBookToCollectionAsync(string userId, int id)
        {
            if (!context.UsersBooks.Any(ub => ub.CollectorId == userId && ub.BookId == id))
            {
                var entry = new IdentityUserBook()
                {
                    CollectorId = userId,
                    BookId = id
                };

                await context.UsersBooks.AddAsync(entry);
                await context.SaveChangesAsync();
            }
        }

        public async Task<DetailsBookViewModel> CreateDetailsModelAsync(int id)
        {
            var book = await context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new DetailsBookViewModel()
                {
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Category = b.Category.Name
                })
                .FirstAsync();

            return book;
        }

        public async Task CreateNewEntityAsync(BookFormModel model)
        {
            var entityToAdd = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.Url,
                Rating = model.Rating,
                CategoryId = model.CategoryId
            };

            await context.Books.AddAsync(entityToAdd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var bookToDelete = await context.Books
                .FirstAsync(b => b.Id == id);

            context.Books.Remove(bookToDelete);
            await context.SaveChangesAsync();
        }

        public async Task EditBookAsync(BookFormModel model, int id)
        {
            var entityToEdit = await context.Books
                .FirstAsync(b => b.Id == id);

            entityToEdit.Title = model.Title;
            entityToEdit.Author = model.Author;
            entityToEdit.Description = model.Description;
            entityToEdit.ImageUrl = model.Url;
            entityToEdit.Rating = model.Rating;
            entityToEdit.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync()
        {
            var allBooks = await context.Books
                .AsNoTracking()
                .Select(b => new AllBookViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Category = b.Category.Name
                })
                .ToListAsync();

            return allBooks;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<BookFormModel> GetEditBookModelAsync(int id)
        {
            var bookToEdit = await context.Books
                .AsNoTracking()
                .FirstAsync(b => b.Id == id);

            var editModel = new BookFormModel()
            {
                Title = bookToEdit.Title,
                Author = bookToEdit.Author,
                Description = bookToEdit.Description,
                Url = bookToEdit.ImageUrl,
                CategoryId = bookToEdit.CategoryId,
                Rating = bookToEdit.Rating
            };

            return editModel;
        }

        public async Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId)
        {
            var mineBooks = await context.UsersBooks
                .AsNoTracking()
                .Where(ub => ub.CollectorId == userId)
                .Select(ub => new MineBookViewModel()
                {
                    Id = ub.Book.Id,
                    Title = ub.Book.Title,
                    Author = ub.Book.Author,
                    Description = ub.Book.Description,
                    ImageUrl = ub.Book.ImageUrl,
                    Category = ub.Book.Category.Name
                })
                .ToListAsync();

            return mineBooks;
        }

        public async Task<bool> IsBookExistingAsync(int id)
        {
            var bookToBeChecked = await context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookToBeChecked == null)
            {
                return false;
            }

            return true;
        }

        public async Task RemoveBookFromCollectionAsync(string userId, int id)
        {
            if (context.UsersBooks.Any(ub => ub.CollectorId == userId && ub.BookId == id))
            {
                var bookToRemove = await context.UsersBooks
                    .FirstAsync(ub => ub.CollectorId == userId && ub.BookId == id);

                context.UsersBooks.Remove(bookToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}
