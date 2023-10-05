using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);

        Task<UserUpdateDto> GetUserByUsernameASync(string username);

        Task<SignInResult> CheckUserPasswordASync(UserUpdateDto userUpdateDto, string password);

        Task<UserDto> CreateAccountASync(UserDto userDto);
        
        Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);
    }
}