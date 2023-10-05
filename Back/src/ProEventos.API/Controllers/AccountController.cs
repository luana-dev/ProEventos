using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService,
                                 ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(){
            try{
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUsernameASync(userName);
                return Ok(user);

            } catch(Exception ex){

                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto){
            try{
                if (await _accountService.UserExists(userDto.Username))
                    return BadRequest("Usuário já existe");

                var user = await _accountService.CreateAccountASync(userDto);
                if (user != null)
                    return Ok(user);
                

                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            } catch(Exception ex){

                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar cadastrar usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin){
            try{
                var user = await _accountService.GetUserByUsernameASync(userLogin.Username);
                if(user == null) return Unauthorized("Usuário ou senha inválidos!");
                
                var result = await _accountService.CheckUserPasswordASync(user, userLogin.Password);
                if (!result.Succeeded) return Unauthorized();

                return Ok(new
                {
                    userName = user.Username,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });

            } catch(Exception ex){

                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao realizar login do usuário. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto){
            try{
                var user = await _accountService.GetUserByUsernameASync(User.GetUserName());
                if(user == null) return Unauthorized("Usuário inválido!");
                
                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return NoContent();

                return Ok(userReturn);

            } catch(Exception ex){

                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }
    }
}