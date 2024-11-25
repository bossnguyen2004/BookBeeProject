using BookBee.Model;

namespace BookBee.Persistences.Repositories.CategoryRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _dataContext;
        public int Total { get; set; }
        public TagRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateCategorys(Tag tag)
        {
            _dataContext.Tags.Add(tag);

        }

        public void DeleteCategorys(Tag tag)
        {
            _dataContext.Tags.Remove(tag);
        }

        public List<Tag> GetCategorys(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var query = _dataContext.Tags.Where(r => r.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(au => au.Name.ToLower().Contains(key.ToLower()));
            }

            switch (sortBy)
            {
                case "NAME":
                    query = query.OrderBy(u => u.Name);
                    break;
                default:
                    query = query.OrderBy(u => u.IsDeleted).ThenBy(u => u.Id);
                    break;
            }
            Total = query.Count();
            if (page == null || pageSize == null || sortBy == null) { return query.ToList(); }
            else
                return query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        public Tag GetCategorysById(int id)
        {
            return _dataContext.Tags.FirstOrDefault(t => t.Id == id);
        }

        public int GetCategorysCount()
        {
            return _dataContext.Tags.Count();
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void UpdateCategorys(Tag tag)
        {
            tag.Update = DateTime.Now;
            _dataContext.Tags.Update(tag);

        }
    }
}
