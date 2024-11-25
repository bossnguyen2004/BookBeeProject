using BookBee.Model;

namespace BookBee.Persistences.Repositories.AuthorRepository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _dataContext;
        public int Total { get; set; }
        public AuthorRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateAuthor(Author author)
        {
            _dataContext.Authors.Add(author); 

        }

        public void DeleteAuthor(Author author)
        {
            _dataContext.Authors.Remove(author);
        }

        public Author GetAuthorById(int id)
        {
            return _dataContext.Authors.FirstOrDefault(a => a.Id == id);
        }

        public int GetAuthorCount()
        {
            return _dataContext.Authors.Count();
        }

        public List<Author> GetAuthors(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var query = _dataContext.Authors.Where(r => r.IsDeleted == false).AsQueryable();

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

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void UpdateAuthor(Author author)
        {
            author.Update = DateTime.Now;
            _dataContext.Authors.Update(author);

        }
    }
}
