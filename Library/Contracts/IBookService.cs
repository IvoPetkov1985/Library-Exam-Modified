using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task AddBookToCollectionAsync(string userId, int id);

        Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId);

        Task CreateNewEntityAsync(BookFormModel model);

        Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync();

        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<bool> IsBookExistingAsync(int id);

        Task RemoveBookFromCollectionAsync(string userId, int id);

        Task<BookFormModel> GetEditBookModelAsync(int id);

        Task EditBookAsync(BookFormModel model, int id);

        Task DeleteBookAsync(int id);

        Task<DetailsBookViewModel> CreateDetailsModelAsync(int id);
    }
}
