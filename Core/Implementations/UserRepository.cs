using Core.Services;
using Entity.DTOModels;
using Entity.Helper;
using Entity.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implementations
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IValidations _validations;
        private readonly IWalletRepository _walletRepository;
        private readonly ILoggerManager _logger;

        public UserRepository(ApplicationContext applicationContext, IValidations validations,
            IWalletRepository walletRepository, ILoggerManager logger)
        {
            _applicationContext = applicationContext;
            _validations = validations;
            _walletRepository = walletRepository;
            _logger = logger;
        }
        public async Task <bool> Login(UserLoginDto user)
        {
            try
            {
                var oldUser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email.ToLower());
                if (oldUser == null || oldUser.Password != user.Password)
                {
                    throw new NullReferenceException();
                    throw new Exception();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");
            }
            return false;
        }

        public async Task<bool> Register(UserRegistionDto userRegistionDto)
        {
            try
            {
                if (_validations.IsValidEmail(userRegistionDto.Email) && _validations.IsValidUsername(userRegistionDto.UserName)
                && _validations.IsValidPassword(userRegistionDto.Password) && _validations.IsValidName(userRegistionDto.FirstName)
                && _validations.IsValidName(userRegistionDto.LastName))
                {

                    var newUser = new UserModel()
                    {
                        UserId = Guid.NewGuid().ToString().Substring(0, 11),
                        FirstName = userRegistionDto.FirstName.ToUpper(),
                        LastName = userRegistionDto.LastName.ToUpper(),
                        Email = userRegistionDto.Email.ToLower(),
                        Password = userRegistionDto.Password,
                        UserName = userRegistionDto.UserName.ToLower()
                    };

                    var newwallet = new WalletModel()
                    {
                        WalletAddress = Guid.NewGuid().ToString().Substring(0, 10),
                        User = newUser
                    };

                    await _applicationContext.AddAsync(newUser);
                    await _applicationContext.AddAsync(newwallet);

                    await _applicationContext.SaveChangesAsync();
                    return true;
                }
                throw new InvalidOperationException();

            }
            catch(Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");
                
            }
            
            return false;


        }

        public async Task<bool> RemoveUser(string userid)
        {
            
            try
            {
                var removeduser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserName == userid);

                if (removeduser == null)
                {
                    throw new NullReferenceException();
                    throw new ArgumentNullException(nameof(removeduser));
                }

                _applicationContext.Remove(removeduser);
                _applicationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return false;
        }

        public async Task<bool> UpdateUser(UserRegistionDto user)
        {
            
            try
            {
                var editedUser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (editedUser != null && _validations.IsValidName(user.FirstName)
                    && _validations.IsValidName(user.LastName) && _validations.IsValidPassword(user.Password))
                {
                    editedUser.UserName = user.UserName.ToLower();
                    editedUser.LastName = user.LastName.ToUpper();
                    editedUser.FirstName = user.FirstName.ToUpper();
                    editedUser.Email = user.Email.ToLower();

                    _applicationContext.Update(editedUser);
                    _applicationContext.SaveChanges();

                    return true;
                }
                throw new NullReferenceException();
                throw new ArgumentNullException(nameof(editedUser)); 
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return false;
        }

        public async Task<UserRegistionDto> GetUserByUserName(string username)
        {
            
            try
            {
                var pickedUser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

                if (pickedUser == null)
                {
                    throw new NullReferenceException();
                }
                return new UserRegistionDto
                {
                    UserName = pickedUser.UserName,
                    LastName = pickedUser.LastName,
                    FirstName = pickedUser.FirstName,
                    Email = pickedUser.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return new UserRegistionDto();
        }

        [Obsolete]
        public async Task<PageList<UserRegistionDto>> GetAllUsers(PaginatedParameters parameters)
        {
            
            try
            {
                var pickedUser = await _applicationContext.Users.Select(x => new UserRegistionDto
                {
                    UserName = x.UserName,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    Email = x.Email
                }).ToListAsync();

                if (pickedUser == null)
                    throw new ExecutionEngineException();
                return PageList<UserRegistionDto>.ToPagedList(pickedUser,parameters.PageNumber,parameters.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return null;

        }
    }
}
