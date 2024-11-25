using BookBee.DTOs.Response;

namespace BookBee.Services.AuthorService
{
    public interface IAuthorService
    {
        ResponseDTO GetAuthors(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        ResponseDTO GetAuthorById(int id);
        ResponseDTO UpdateAuthor(int id, string name,int? deathYear = null);
        ResponseDTO DeleteAuthor(int id);
        ResponseDTO CreateAuthor(string name, int? birthYear = null, int? deathYear = null);
    }
}
