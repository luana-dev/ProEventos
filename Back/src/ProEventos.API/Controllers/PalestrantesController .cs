using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public PalestrantesController(IPalestranteService palestranteService, 
                                      IWebHostEnvironment hostEnvironment,
                                      IAccountService accountService){

            _palestranteService = palestranteService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams){
            try
            {
                var palestrante = await _palestranteService.GetAllPalestrantesASync(pageParams, true);
                if(palestrante == null) return NoContent();

                Response.AddPagination(
                    palestrante.CurrentPage, 
                    palestrante.PageSize, 
                    palestrante.TotalCount, 
                    palestrante.TotalPages);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdASync(User.GetUserId(), true);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdASync(User.GetUserId(), false);

                if(palestrante == null)
                    palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(),model);
                
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteService.UpdatePalestrantes(User.GetUserId(), model);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar palestrantes. Erro: {ex.Message}");
            }
        }
    }
}
