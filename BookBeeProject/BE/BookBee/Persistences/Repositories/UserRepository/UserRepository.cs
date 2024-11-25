using BookBee.Model;
using Microsoft.EntityFrameworkCore;

namespace BookBee.Persistences.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int Total { get; set; }
        public void CreateUser(User user)
        {
            _dataContext.Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }

        public User GetUserById(int id)
        {
            return _dataContext.Users.Include(r => r.Role).FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _dataContext.Users.Include(r => r.Role).FirstOrDefault(u => u.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return _dataContext.Users.Include(r => r.Role).FirstOrDefault(u => u.Email == email);
        }

        public int GetUserCount()
        {
            return _dataContext.Users.Count();
        }

        public List<User> GetUsers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", bool includeDeleted = false)
        {
            var query = _dataContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(key.ToLower()) || u.Username.ToLower().Contains(key.ToLower()) || u.Email.ToLower().Contains(key.ToLower()) || u.Phone.ToLower().Contains(key.ToLower()));
            }

            switch (sortBy)
            {
                case "NAME":
                    query = query.OrderBy(u => u.LastName);
                    break;
                default:
                    query = query.OrderBy(u => u.IsDeleted).ThenByDescending(u => u.Create);
                    break;
            }

            if (!includeDeleted)
            {
                query = query.Where(r => !r.IsDeleted);
            }

            Total = query.Count();

            if (page == null || pageSize == null || sortBy == null) { return query.ToList(); }
            else
                return query.Where(r => r.Username != "guest").Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void UpdateUser(User user)
        {
            user.Update = DateTime.Now;
            _dataContext.Users.Update(user);
        }
    }
}
