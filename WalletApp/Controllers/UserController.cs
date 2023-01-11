using Core.Services;
using Entity.DTOModels;
using Entity.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace WalletApp.Controllers
{
    [Route("api/v{versions:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public UserController(IUserRepository user)
        {
            _user = user;
        }
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="register"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Register-User")]
        public async Task<IActionResult> AddUser([FromForm] UserRegistionDto register)
        {
            var registration = await _user.Register(register);

            return registration == null ? BadRequest("registration not successful") : Ok(registration);
        }
        /// <summary>
        /// Give users access to the database
        /// </summary>
        /// <param name="login"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("UserLogin")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto login)
        {
            var LoginUser = await _user.Login(login);

            return LoginUser == null ? BadRequest("registration not successful") : Ok(LoginUser);
        }
        /// <summary>
        /// Enables users to update their information
        /// </summary>
        /// <param name="details"></param>
        /// <returns>IActionResult</returns>
        [HttpPut("Update-User-Details")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRegistionDto details)
        {
            var update = await _user.UpdateUser(details);

            return update ? Ok("Update Successful") : BadRequest("registration not successful");
        }
        /// <summary>
        /// Return all user in the database
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery]PaginatedParameters parameters)
        {
            var update = await _user.GetAllUsers(parameters);

            return Ok(APIListResponseDto.Success(update, parameters.PageNumber, update.MetaData.PageSize, update.MetaData.TotalPages, update.MetaData.TotalCount,
            update.MetaData.HasPrevious, update.MetaData.HasNext));
        }
        /// <summary>
        /// Return user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IActtionResult</returns>
        [HttpGet("Get-User-By-Username")]
        public async Task<IActionResult> GetUserByUsername([FromQuery]string username)
        {
            var user = await _user.GetUserByUserName(username);

            return Ok(user);
        }
        /// <summary>
        /// Delete User by user Id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>IActonResult</returns>
        [HttpDelete("Remove-User")]
        public async Task<IActionResult> RemoveUser([FromQuery] string userid)
        {
            var user = await _user.RemoveUser(userid);

            return Ok(user);
        }
    }
}
