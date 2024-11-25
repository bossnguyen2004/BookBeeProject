using BookBee.DTOs.Response;
using BookBee.DTOs.User;

namespace BookBee.Services.UserService
{
    public interface IUserService
    {
        ResponseDTO GetUsers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", bool includeDeleted = false);
        ResponseDTO GetUserById(int id);
        ResponseDTO GetPersonalInfo();
        ResponseDTO GetUserByUsername(string username);
        ResponseDTO UpdateUser(int id, UpdateUserDTO updateUserDTO);
        ResponseDTO SelfUpdateUser(UpdateUserDTO updateUserDTO);
        ResponseDTO DeleteUser(int id);
        ResponseDTO RestoreUser(int id);
        ResponseDTO CreateUser(CreateUserDTO createUserDTO);
    }
}
