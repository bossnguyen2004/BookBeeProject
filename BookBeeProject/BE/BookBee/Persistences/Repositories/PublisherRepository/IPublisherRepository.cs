using BookBee.Model;

namespace BookBee.Persistences.Repositories.PublisherRepository
{
    public interface IPublisherRepository
    {
        List<Publisher> GetPublishers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        Publisher GetPublisherById(int id);
        void UpdatePublisher(Publisher publisher);
        void DeletePublisher(Publisher publisher);
        void CreatePublisher(Publisher publisher);
        int GetPublisherCount();
        bool IsSaveChanges();
        int Total { get; }
    }
}
