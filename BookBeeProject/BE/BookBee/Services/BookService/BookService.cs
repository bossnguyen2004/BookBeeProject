using AutoMapper;
using BookBee.DTOs.Book;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.AuthorRepository;
using BookBee.Persistences.Repositories.BookRepository;
using BookBee.Persistences.Repositories.CategoryRepository;
using BookBee.Persistences.Repositories.PublisherRepository;

namespace BookBee.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository, IMapper mapper, ITagRepository tagRepository,
            IAuthorRepository authorRepository, IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }
        public ResponseDTO CreateBook(
              string title,
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
              List<int> tagIds)
        {
            // Kiểm tra Publisher
            var publisher = _publisherRepository.GetPublisherById(publisherId);
            if (publisher == null)
                return new ResponseDTO { Code = 400, Message = "Nhà xuất bản không tồn tại." };

            // Kiểm tra Author
            var author = _authorRepository.GetAuthorById(authorId);
            if (author == null)
                return new ResponseDTO { Code = 400, Message = "Tác giả không tồn tại." };

            // Kiểm tra nếu tagIds không hợp lệ hoặc rỗng
            if (tagIds == null || tagIds.Count == 0)
                return new ResponseDTO { Code = 400, Message = "Danh sách Tag không được để trống." };

            // Khởi tạo đối tượng Book
            var book = new Book
            {
                Title = title,
                Description = description,
                NumberOfPages = numberOfPages,
                PublishDate = publishDate,
                Language = language,
                Count = count,
                Price = price,
                Image = image,
                Format = format,
                PageSize = pageSize,
                Publisher = publisher,
                Author = author,
                Tags = new List<Tag>() // Khởi tạo List<Tag> rỗng
            };

            // Thêm các Tag vào sách nếu tồn tại
            foreach (var tagId in tagIds)
            {
                var tag = _tagRepository.GetCategorysById(tagId);
                if (tag != null)
                {
                    book.Tags.Add(tag);
                }
                else
                {
                    // Trả về thông báo nếu Tag không tồn tại
                    return new ResponseDTO { Code = 400, Message = $"Tag với ID {tagId} không tồn tại." };
                }
            }

            // Kiểm tra nếu không có Tag được thêm vào
            if (book.Tags.Count == 0)
                return new ResponseDTO { Code = 400, Message = "Không có Tag hợp lệ được thêm vào sách." };

            // Lưu vào database
            _bookRepository.CreateBook(book);
            if (_bookRepository.IsSaveChanges())
            {
                // Trả về thông tin sách đã tạo thành công
                return new ResponseDTO
                {
                    Code = 200,
                    Message = "Tạo sách thành công.",
                    Data = new { book.Id, book.Title, book.Tags.Count } // Trả về một số thông tin hữu ích
                };
            }

            return new ResponseDTO { Code = 400, Message = "Tạo sách thất bại." };
        }

        public ResponseDTO DeleteBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Book không tồn tại"
            };

            book.IsDeleted = true;

            _bookRepository.UpdateBook(book);
            if (_bookRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Xóa thành công"
                };
            else
                return new ResponseDTO()
                {
                    Data = 400,
                    Message = "Xóa thất bại"
                };
        }

        public ResponseDTO RestoreBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Book không tồn tại"
            };

            book.IsDeleted = false;
            _bookRepository.UpdateBook(book);
            if (_bookRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Khôi phục thành công"
                };
            else
                return new ResponseDTO()
                {
                    Data = 400,
                    Message = "Khôi phục thất bại"
                };
        }

        public ResponseDTO GetBookById(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Book không tồn tại"
            };

            var data = _mapper.Map<BookDTO>(book);
            return new ResponseDTO()
            {
                Data = data
            };
        }
        public ResponseDTO GetBookByIds(List<int> ids)
        {
            var books = new List<Book>();
            foreach (int id in ids)
            {
                var book = _bookRepository.GetBookById(id);
                if (book != null) books.Add(book);
            }
            if (books == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Book không tồn tại"
            };

            return new ResponseDTO()
            {
                Data = _mapper.Map<List<BookDTO>>(books)
            };
        }

        public ResponseDTO GetBooks(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", int? tagId = 0, bool includeDeleted = false, int? publisherId = null, int? authorId = null)
        {
            var books = _bookRepository.GetBooks(page, pageSize, key, sortBy, tagId, includeDeleted, publisherId, authorId);

            return new ResponseDTO()
            {
                Data = _mapper.Map<List<BookDTO>>(books),
                Total = _bookRepository.Total
            };
        }

        public ResponseDTO GetBookRecommendations()
        {
            var books = _bookRepository.GetBooks();
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<BookDTO>>(books),
                Total = _bookRepository.GetBookCount()
            };
        }

      

        public ResponseDTO GetTopOrderedBooks(int topCount = 10)
        {
            var books = _bookRepository.GetTopOrderedBooks(topCount);
            var data = _mapper.Map<List<BookDTO>>(books);
            return new ResponseDTO()
            {
                Data = data,
                Total = data.Count
            };
        }

        public ResponseDTO GetCart(List<int> bookIds)
        {
            var books = _bookRepository.GetCart(bookIds);

            return new ResponseDTO()
            {
                Data = _mapper.Map<List<BookDTO>>(books),
                Total = _bookRepository.GetBookCount()
            };
        }

        public ResponseDTO UpdateBook(int id, UpdateBookDTO updateBookDTO)
        {

            var book = _bookRepository.GetBookById(id);
            if (book == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Book không tồn tại"
            };

            book.Update = DateTime.Now;
            book.Title = updateBookDTO.Title;
            book.Description = updateBookDTO.Description;
            book.NumberOfPages = updateBookDTO.NumberOfPages;
            book.PublishDate = updateBookDTO.PublishDate;                          
            book.Language = updateBookDTO.Language;
            book.Count = updateBookDTO.Count;
            book.Price = updateBookDTO.Price;
            book.Image = updateBookDTO.Image;
            book.Format = updateBookDTO.Format;
            book.PageSize = updateBookDTO.PageSize;

            book.PublisherId = updateBookDTO.PublisherId;
            book.AuthorId = updateBookDTO.AuthorId;

            book.Tags = new List<Tag>();
            foreach (var tagId in updateBookDTO.TagIds)
            {
                Tag tag = _tagRepository.GetCategorysById(tagId);
                if (tag != null)
                    book.Tags.Add(tag);
            }

            _bookRepository.UpdateBook(book);
            if (_bookRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Cập nhật thành công"
                };
            return new ResponseDTO()
            {
                Data = 400,
                Message = "Cập nhật thất bại"
            };
        }
    }
}
