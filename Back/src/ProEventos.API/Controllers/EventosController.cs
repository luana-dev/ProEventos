﻿using System;
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
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventosService;

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public EventosController(IEventosService eventosService, 
                                 IWebHostEnvironment hostEnvironment,
                                 IAccountService accountService){
            _eventosService = eventosService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams){
            try
            {
                var eventos = await _eventosService.GetAllEventosASync(User.GetUserId(), pageParams, true);
                if(eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdASync(User.GetUserId(),id, true);
                if(evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventosService.AddEventos(User.GetUserId(),model);
                if(evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdASync(User.GetUserId(),eventoId, true);
                if(evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }
                var eventoRetorno = await _eventosService.UpdateEventos(User.GetUserId(),eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventosService.UpdateEventos(User.GetUserId(),id, model);
                if(evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdASync(User.GetUserId(),id, true);
                if(evento == null) return NoContent();

                if(await _eventosService.DeleteEvento(User.GetUserId(),id)){

                    DeleteImage(evento.ImagemURL);
                    return Ok(new {message = "Deletado"});

                } else {
                    throw new Exception("Ocorreu um problema ao deletar o evento.");
                }
            }
            catch (Exception ex)
            {
             return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar evento. Erro: {ex.Message}");   
            }
        }

         [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile){
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                              .Take(10)
                                              .ToArray()
                                          ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName){
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
            if(System.IO.File.Exists(imagePath)){
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
