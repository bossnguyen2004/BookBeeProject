
using BookBee.DTOs.Response;
using BookBee.DTOs.Tag;

namespace BookBee.Services.CategoryService
{
    public interface ITagService
    {
        ResponseDTO GetCategorys(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        ResponseDTO GetCategorysById(int id);
        ResponseDTO UpdateCategorys(int id, TagRequestDTO tagRequestDto);
        ResponseDTO DeleteCategorys(int id);
        ResponseDTO CreateCategorys(string name, string image);
    }
}
