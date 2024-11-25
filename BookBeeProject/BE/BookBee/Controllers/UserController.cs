using BookBee.DTOs.User;
using BookBee.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUserById(int id)
        {
            var res = _userService.GetUserById(id);
            return StatusCode(res.Code, res);
        }

        [HttpGet("Personal")]
        public IActionResult GetPersonalInfo()
        {
            var res = _userService.GetPersonalInfo();
            return StatusCode(res.Code, res);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", bool includeDeleted = false)
        {
            var res = _userService.GetUsers(page, pageSize, key, sortBy, includeDeleted);
            return StatusCode(res.Code, res);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateUser(int id, UpdateUserDTO updateUserDTO)
        {
            var res = _userService.UpdateUser(id, updateUserDTO);
            return StatusCode(res.Code, res);
        }

        [HttpPut("Self")]
        public IActionResult SelfUpdateUser(UpdateUserDTO updateUserDTO)
        {
            var res = _userService.SelfUpdateUser(updateUserDTO);
            return StatusCode(res.Code, res);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteUser(int id)
        {
            var res = _userService.DeleteUser(id);
            return StatusCode(res.Code, res);
        }
        [HttpPut("{id}/restore")]
        [Authorize(Roles = "admin")]
        public IActionResult RestoreUser(int id)
        {
            var res = _userService.RestoreUser(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateUser(CreateUserDTO createUserDTO)
        {
            var res = _userService.CreateUser(createUserDTO);
            return StatusCode(res.Code, res);
        }
    }
}
