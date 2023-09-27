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
        private readonly IOptions<DownloadConfiguration> _downloadConfiguration;


        public DownloadService(IHttpClientFactory httpClientFactory, IOptions<DownloadConfiguration> downloadConfiguration)//wtf
        {
            this._httpClientFactory = httpClientFactory;
            this._downloadConfiguration = downloadConfiguration;
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

                    string result = Regex.Replace(extractedFileName, "^\"|\"$", "");
                }
                else
                {
                    return;
                }

                //var content = await response.Content.ReadAsByteArrayAsync();

                string fileExtension = GetFileExtensionFromMediaType(response.Content.Headers.ContentType);
                var createdFileName = $"{Guid.NewGuid()}.{fileExtension}";
                #endregion
                var buffer = new byte[1024 * 1024]; // 1 MB
                var totalBytesRead = 0;
                var totalBytesToRead = response.Content.Headers.ContentLength.Value;
                using (var fileStream = File.Create(Path.Combine(_downloadConfiguration.Value.DownloadPath, extractedFileName ?? createdFileName)))
                {

                    // Read the content of the response into a byte array
                    var content = await response.Content.ReadAsByteArrayAsync();

                    while (totalBytesRead < totalBytesToRead)
                    {
                        // Copy the contents of the byte array to the file stream
                        await fileStream.WriteAsync(content, totalBytesRead, content.Length - totalBytesRead);
                        totalBytesRead += content.Length - totalBytesRead;
                    }
                }
            }
        }
        public async Task DownloadLargeFileAsync(string link)
        {
            // Replace with the actual URL of the large file you want to download

            //using var client = new HttpClient();
            //link = Uri.UnescapeDataString(link);
            //client.MaxResponseContentBufferSize = Int32.MaxValue;

            //// Stream the file content from the remote URL
            //using var responseStream = await client.GetStreamAsync(link);

            //// Read the file content into a byte array
            //using var memoryStream = new MemoryStream();
            //var bufferSize = 4 * 1024 * 1024;
            //var buffer = new byte[bufferSize];

            //int bytesRead;
            //while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            //{
            //    // Write the buffer to the memory stream
            //    await memoryStream.WriteAsync(buffer, 0, bytesRead);
            //}
            ////await responseStream.CopyToAsync(memoryStream);
            //memoryStream.Seek(0, SeekOrigin.Begin);
            //return memoryStream;
            using var client = new HttpClient();
            link = Uri.UnescapeDataString(link);

            // Stream the file content from the remote URL directly to a local file
            //using var responseStream = await client.GetStreamAsync(link);
            //var downloadPath = _downloadConfiguration.Value.DownloadPath;

            //// Check if the directory exists, and create it if not
            //if (!Directory.Exists(downloadPath))
            //{
            //    Directory.CreateDirectory(downloadPath);
            //}


            //using var fileStream = File.Create(downloadPath);

            //await responseStream.CopyToAsync(fileStream);

            //var fileInfo = new FileInfo(_downloadConfiguration.Value.DownloadPath);

            //if (!fileInfo.Exists)
            //    return NotFound();

            //var response = new FileStreamResult(System.IO.File.OpenRead(_downloadConfiguration.Value.DownloadPath), "application/octet-stream")
            //{
            //    FileDownloadName = response.Content.Headers.ContentDisposition?.FileName;,
            //    EnableRangeProcessing = true // Enable support for range requests
            //};

            //Response.Headers.Add("Accept-Ranges", "bytes");
            //Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            //return response;
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
