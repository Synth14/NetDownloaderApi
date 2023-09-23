using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Models;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace NetDownloaderApi.Services
{

    public class DownloadService : IDownloadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DownloadConfiguration _configuration;
        private readonly string _downloadPath;


        public DownloadService(IHttpClientFactory httpClientFactory, DownloadConfiguration configuration, IOptions<DownloadConfiguration> downloadConfig)//wtf
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration; //wtf
            this._downloadPath = downloadConfig.Value.DownloadPath;//work, but eww
        }

        public async Task DownloadAsync(string link)
        {
            var client = _httpClientFactory.CreateClient();
            link = Uri.UnescapeDataString(link);
            client.MaxResponseContentBufferSize = 2147483647;
            var response = await client.GetAsync(link);
           
        
            if (response.IsSuccessStatusCode)
            {
                #region Filename handling
                var extractedFileName = response.Content.Headers.ContentDisposition?.FileName;// <= ça intéressant pour nom de fichier
                if (!string.IsNullOrEmpty(extractedFileName))
                {
                    string sanitized = extractedFileName.TrimStart('\\').TrimEnd('"');
                }
                    

                string result = Regex.Replace(extractedFileName, "^\"|\"$", "");
                //var content = await response.Content.ReadAsByteArrayAsync();

                var extension = response.Content.Headers.ContentType;
                string fileExtension = GetFileExtensionFromMediaType(extension);
                var createdFileName = $"{Guid.NewGuid()}.{fileExtension}";
                #endregion
                var buffer = new byte[1024 * 1024]; // 1 MB
                var totalBytesRead = 0;
                var totalBytesToRead = response.Content.Headers.ContentLength.Value;
                using (     var fileStream = File.Create(Path.Combine(_configuration.DownloadPath ??_downloadPath, extractedFileName ?? createdFileName)))
                {

                    // Read the content of the response into a byte array
                    var content = await response.Content.ReadAsByteArrayAsync();

                    while (totalBytesRead < totalBytesToRead)
                    {
                        // Copy the contents of the byte array to the file stream
                        await fileStream.WriteAsync(content, totalBytesRead, content.Length - totalBytesRead);
                        totalBytesRead += content.Length - totalBytesRead;
                    }
                //using (var fileStream = File.Create(Path.Combine(_configuration.DownloadPath ?? _downloadPath, uniqueFileName)))
                //{
                //    await fileStream.WriteAsync(content);
                }
                
            }
        }
        public async Task<MemoryStream> DownloadLargeFileAsync(string link)
        {
            // Replace with the actual URL of the large file you want to download

            using var client = new HttpClient();
            link = Uri.UnescapeDataString(link);

            // Stream the file content from the remote URL
            var responseStream = await client.GetStreamAsync(link);

            // Read the file content into a byte array
            using var memoryStream = new MemoryStream();
            memoryStream.Seek(0, SeekOrigin.Begin);
            await responseStream.CopyToAsync(memoryStream);
            return memoryStream;
        }
        public static string GetFileExtensionFromMediaType(MediaTypeHeaderValue contentType)
        {
            // Check if the ContentType is not null and has a MediaType
            if (contentType != null && !string.IsNullOrEmpty(contentType.MediaType))
            {
                // Split the MediaType by '/' and get the last part as the file extension
                string[] parts = contentType.MediaType.Split('/');
                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }

            // Return an empty string or handle the case when there's no valid extension
            return string.Empty;
        }
    }
}
