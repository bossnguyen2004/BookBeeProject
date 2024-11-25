using BookBee.Model;
using Microsoft.EntityFrameworkCore;

namespace BookBee.Persistences.Repositories.BookRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _dataContext;

        public int Total { get; private set; }
        public BookRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void CreateBook(Book book)
        {
            _dataContext.Books.Add(book);

        }

        public void DeleteBook(Book book)
        {
            _dataContext.Books.Remove(book);
        }

        public Book GetBookById(int id)
        {
            return _dataContext.Books
                    .Include(a => a.Author).
                    Include(p => p.Publisher)
                    .Include(t => t.Tags)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(b => b.Order)
                    .AsSplitQuery()
                    .FirstOrDefault(b => b.Id == id);
        }

        public int GetBookCount()
        {
            return _dataContext.Books.Count();
        }

        public List<Book> GetBooks(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", int? categoryId = 0, bool includeDeleted = false, int? publisherId = null, int? authorId = null)
        {
            var query = _dataContext.Books
                .Include(a => a.Author)
                .Include(p => p.Publisher)
                .Include(t => t.Tags)
                .Include(o => o.OrderDetails)
                .ThenInclude(b => b.Order)
                .AsSplitQuery()
                .AsQueryable();

            var tag = _dataContext.Tags.FirstOrDefault(t => t.Id == categoryId);

            if (tag != null)
            {
                query = query.Where(b => b.Tags.Any(t => t.Id == categoryId));
            }

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(au => au.Title.ToLower().Contains(key.ToLower()));
            }

            if (publisherId.HasValue)
            {
                query = query.Where(b => b.PublisherId == publisherId.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            switch (sortBy.ToUpper())
            {
                case "TITLE":
                    query = query.OrderBy(u => u.Title);
                    break;
                case "PRICE":
                    query = query.OrderBy(u => u.Price);
                    break;
                case "PRICE_DEC":
                    query = query.OrderByDescending(u => u.Price);
                    break;
                case "PUBLISHDATE":
                    query = query.OrderBy(u => u.PublishDate);
                    break;
                default:
                    query = query.OrderBy(u => u.IsDeleted).ThenBy(u => u.Id);
                    break;
            }

            if (!includeDeleted)
            {
                query = query.Where(b => !b.IsDeleted);
            }

            Total = query.Count();

            if (page == null || pageSize == null || sortBy == null)
            {
                return query.ToList();
            }
            else
            {
                return query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }
        }

       
        public List<Book> GetTopOrderedBooks(int topCount = 10)
        {
            var topOrderedBooks = _dataContext.Books
                .Include(b => b.OrderDetails)
                .ThenInclude(ob => ob.Order)
                .Where(b => b.OrderDetails.Any(ob => ob.Order.Status == "DON" && !ob.Order.IsDeleted) && !b.IsDeleted)
                .Select(b => new
                {
                    Book = b,
                    TotalQuantity = b.OrderDetails
                        .Where(ob => ob.Order.Status == "DON" && !ob.Order.IsDeleted)
                        .Sum(ob => ob.Quantity)
                })
                .OrderByDescending(b => b.TotalQuantity)
                .Take(topCount)
                .Select(b => b.Book)
                .ToList();

            return topOrderedBooks;
        }

        public List<Book> GetCart(List<int> bookIds)
        {
            List<Book> books = new List<Book>();
            foreach (var bookId in bookIds)
            {
                var book = _dataContext.Books.FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                    books.Add(book);
            }
            return books;
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void UpdateBook(Book book)
        {
            book.Update = DateTime.Now;
            _dataContext.Books.Update(book);
        }
    }
}
