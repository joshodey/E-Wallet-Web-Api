using Entity.DTOModels;
using Entity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserRepository
    {
        Task<bool> Register(UserRegistionDto userRegistionDto);
        Task<bool> Login(UserLoginDto user);

        Task<bool> RemoveUser(string user);

        Task<bool> UpdateUser(UserRegistionDto user);

        Task<UserRegistionDto> GetUserByUserName(string username);

        Task<PageList<UserRegistionDto>> GetAllUsers(PaginatedParameters parameter);
    }
}
