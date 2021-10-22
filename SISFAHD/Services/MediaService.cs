using Microsoft.AspNetCore.Http;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SISFAHD.Services
{
    public class MediaService
    {
        private readonly IFileStorage fileStorage;

        public MediaService(IFileStorage fileStorage)
        {
            this.fileStorage = fileStorage;
        }

        public async Task<String> CrearListaArchivos(IFormFile mediaInfo)
        {
            String urlArchivo = "";

            using (var stream = new MemoryStream())
            {
                await mediaInfo.CopyToAsync(stream);

                urlArchivo = await fileStorage.SaveDoc(stream.ToArray(), "pdf", "archivos");

            }

            return urlArchivo;
        }

        public async Task<String> ModificarListaArchivos(IFormFile mediaInfo, string urlarchivo)
        {
            String urlAcrhivo = "";

            using (var stream = new MemoryStream())
            {
                await mediaInfo.CopyToAsync(stream);
                urlAcrhivo = await fileStorage.EditFile(stream.ToArray(), "pdf", "archivos", urlarchivo);
            }

            return urlAcrhivo;
        }

        public async Task EliminarListaArchivos(List<String> listaArchivos)
        {
            String urlImage = "";
            listaArchivos.ForEach(async x =>
            {
                await fileStorage.DeleteFile(x, "archivos");
            });
        }
    }
}
