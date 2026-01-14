using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Room.Me.Services
{
    public class ImageService
    {
        private readonly BlobContainerClient _containerClient;

        public ImageService(IConfiguration configuration)
        {
            // lee el string de conexion 
            var connectionString = configuration.GetConnectionString("AzureStorage");

            var containerName = "fotos-roomme";

            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            // aca se crea el nombre del archivo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = _containerClient.GetBlobClient(fileName);

            // se configuran las cabeceras
            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

            // se sube al azure 
            using (var stream = file.OpenReadStream())
            {

                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }

            // retorna la url
            return blobClient.Uri.ToString();
        }
    }
}