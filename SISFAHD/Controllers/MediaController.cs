﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Models;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly MediaService mediaService;

        public MediaController(MediaService mediaService)
        {
            this.mediaService = mediaService;
        }



        [HttpPut("archivos/pdf/{*urlarchivo}")]
        public async Task<ActionResult<String>> PutArchivo(string urlarchivo, IFormFile file)
        {
            String imageUrl;
            try
            {
                imageUrl = await mediaService.ModificarListaArchivos(file, urlarchivo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return imageUrl;
        }


        [HttpPost("archivos/pdf")]
        public async Task<ActionResult<String>> PostFilePdf(IFormFile file)
        {
            String imageUrl;
            try
            {
                imageUrl = await mediaService.CrearListaArchivos(file);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return imageUrl;
        }
        [HttpPost("archivos/pdf/delete")]
        public async Task<ActionResult<String>> DeleteFilePdf(List<String> listaUrls)
        {
            String imageUrl;
            try
            {
                await mediaService.EliminarListaArchivos(listaUrls);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return "completado";
        }

    }
}