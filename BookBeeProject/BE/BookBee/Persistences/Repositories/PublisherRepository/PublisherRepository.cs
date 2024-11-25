using BookBee.Model;
using Microsoft.EntityFrameworkCore;

namespace BookBee.Persistences.Repositories.PublisherRepository
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DataContext _dataContext;
        public int Total { get; set; }
        public PublisherRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void CreatePublisher(Publisher publisher)
        {
            _dataContext.Publishers.Add(publisher); 
        }

        public void DeletePublisher(Publisher publisher)
        {
            _dataContext.Publishers.Remove(publisher);
        }

        public Publisher GetPublisherById(int id)
        {
            return _dataContext.Publishers.FirstOrDefault(a => a.Id == id);
        }

        public int GetPublisherCount()
        {
            return _dataContext.Publishers.Count();
        }

        public List<Publisher> GetPublishers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {

            var query = _dataContext.Publishers.Where(r => r.IsDeleted == false).AsQueryable();

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

        public void UpdatePublisher(Publisher publisher)
        {
            _dataContext.Entry(publisher).State = EntityState.Modified;
        }
    }
}
