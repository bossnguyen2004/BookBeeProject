using BookBee.DTOs.Book;
using BookBee.DTOs.Response;

namespace BookBee.Services.BookService
{
    public interface IBookService
    {
        ResponseDTO GetBooks(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", int? tagId = 0,
            bool includeDeleted = false, int? publisherId = null, int? authorId = null);
        ResponseDTO GetBookRecommendations();
        ResponseDTO GetTopOrderedBooks(int topCount = 10);
        ResponseDTO GetBookById(int id);
        ResponseDTO GetBookByIds(List<int> ids);
        ResponseDTO UpdateBook(int id, UpdateBookDTO updateBookDTO);
        ResponseDTO DeleteBook(int id);
        ResponseDTO RestoreBook(int id);
        ResponseDTO GetCart(List<int> bookIds);
        ResponseDTO CreateBook(string title,
        string description,
        int numberOfPages,
        DateTime publishDate,
        string language,
        int count,
        double price,
        string image,
        string format,
        string pageSize,
        int publisherId,
        int authorId,
        List<int> tagIds);
    }
}
