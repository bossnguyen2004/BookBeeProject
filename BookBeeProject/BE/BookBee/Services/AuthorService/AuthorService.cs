using AutoMapper;
using BookBee.DTOs.Author;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.AuthorRepository;

namespace BookBee.Services.AuthorService
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public ResponseDTO CreateAuthor(string name, int? birthYear = null, int? deathYear = null)
        {
          
            var author = new Author { Name = name , DateOfBirth = birthYear, DeathYear = deathYear };
            _authorRepository.CreateAuthor(author);
            if (_authorRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Tạo thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Tạo thất bại" };
        }


        public ResponseDTO DeleteAuthor(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Author không tồn tại"
                };
            author.IsDeleted = true;
            _authorRepository.UpdateAuthor(author);
            if (_authorRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }

        public ResponseDTO GetAuthorById(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Author không tồn tại"
                };
            return new ResponseDTO()
            {
                Data = _mapper.Map<AuthorDTO>(author)
            };
        }

        public ResponseDTO GetAuthors(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var authors = _authorRepository.GetAuthors(page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<AuthorDTO>>(authors),
                Total = _authorRepository.Total
            };
        }

        public ResponseDTO UpdateAuthor(int id, string name, int? deathYear = null)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Author không tồn tại"
                };
            author.Update = DateTime.Now;
            author.Name = name;
            if (deathYear.HasValue)
            {
                author.DeathYear = deathYear.Value;
            }
            _authorRepository.UpdateAuthor(author);
            if (_authorRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }
    }
}
