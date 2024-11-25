using BookBee.Model;

namespace BookBee.Persistences.Repositories.UserRepository
{
    public interface IUserRepository
    {
        List<User> GetUsers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", bool includeDeleted = false);
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void CreateUser(User user);
        int GetUserCount();
        bool IsSaveChanges();
        int Total { get; }
    }
}
