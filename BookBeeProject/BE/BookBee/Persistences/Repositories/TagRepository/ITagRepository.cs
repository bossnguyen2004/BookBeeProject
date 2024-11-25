
using BookBee.Model;

namespace BookBee.Persistences.Repositories.CategoryRepository
{
    public interface ITagRepository
    {
        List<Tag> GetCategorys(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        Tag GetCategorysById(int id);
        void UpdateCategorys(Tag tag);
        void DeleteCategorys(Tag tag);
        void CreateCategorys(Tag tag);
        int GetCategorysCount();
        bool IsSaveChanges();
        int Total { get; }
    }
}
