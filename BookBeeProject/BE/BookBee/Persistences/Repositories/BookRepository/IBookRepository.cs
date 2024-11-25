using BookBee.Model;

namespace BookBee.Persistences.Repositories.BookRepository
{
    public interface IBookRepository
    {
        int Total { get; }

        List<Book> GetBooks(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", int? categoryId = 0,
            bool includeDeleted = false, int? publisherId = null, int? authorId = null);
        List<Book> GetCart(List<int> bookIds);
        Book GetBookById(int id);
        void UpdateBook(Book book);
        void DeleteBook(Book book);
        void CreateBook(Book book);
        int GetBookCount();

        List<Book> GetTopOrderedBooks(int topCount = 10);
        bool IsSaveChanges();
    }
}
