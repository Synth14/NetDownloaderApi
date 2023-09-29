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

        public async Task<string> DownloadLargeFileAsync(string fileUrl, string finalFileName)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var tempFileName = Guid.NewGuid().ToString();
                            var tempFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath, tempFileName);

                            using (var fileStream = File.Create(tempFilePath))
                            {
                                var buffer = new byte[8192]; // 8KB buffer
                                var bytesRead = 0;

                                using (var responseStream = await response.Content.ReadAsStreamAsync())
                                {
                                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    }
                                }
                            }

                            var finalFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath, finalFileName);

                            var finalDirectory = Path.GetDirectoryName(finalFilePath);
                            if (!Directory.Exists(finalDirectory))
                            {
                                Directory.CreateDirectory(finalDirectory);
                            }
                            if (tempFilePath != finalFilePath)
                                File.Move(tempFilePath, finalFilePath);

                            if (File.Exists(finalFilePath))
                            {
                                return finalFilePath;
                            }
                            else
                            {
                                File.Delete(tempFilePath);
                                throw new Exception("Failed to rename the file.");
                            }
                        }
                        else
                        {
                            throw new Exception("Download failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
