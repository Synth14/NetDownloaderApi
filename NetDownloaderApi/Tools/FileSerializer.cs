using Microsoft.Extensions.FileSystemGlobbing.Internal;
using NetDownloaderApi.Models;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace NetDownloaderApi.Tools
{
    public class FileSerializer
    {

        public static async Task<string> GetOrCreateFileName(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    string? fileName = null;

                    if (response.IsSuccessStatusCode)
                    {

                        var contentHeaders = response.Content.Headers;
                        //Check in the uri if the filename is provided
                        Uri uri;
                        if (Uri.TryCreate(url, UriKind.Absolute, out uri) && string.IsNullOrEmpty(fileName))
                        {
                            fileName = Path.GetFileName(uri.AbsolutePath);//peut être plus lastindex('\'); sur le uri.LocalPath..a checker
                        }

                        //Check in the response if the filename is provided
                        if (contentHeaders.ContentDisposition != null && !string.IsNullOrEmpty(contentHeaders.ContentDisposition.FileName))
                        {
                            fileName = contentHeaders.ContentDisposition.FileName;
                        }

                        fileName = SanitizeFileName(fileName);

                        if (!Path.HasExtension(fileName)) //peut être move ça dans le controller et l'ajouter à l'identifyFileType, arf
                        {
                            MediaTypeHeaderValue contentType = contentHeaders.ContentType;
                            if (contentType.MediaType != null)
                            {
                                string fileExtension = contentType.MediaType.Split('/').LastOrDefault();

                                if (!string.IsNullOrEmpty(fileExtension))
                                {
                                    return fileName + "." + fileExtension; 
                                }
                            }
                        }
                        return fileName ?? Guid.NewGuid().ToString();
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la requête HTTP. Statut : {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erreur de requête HTTP : {ex.Message}");
                }
            }
            return Guid.NewGuid().ToString();
        }
        private static string SanitizeFileName(string fileName)
        {
            fileName.Trim();
            fileName?.Replace("_", ".");
            return fileName?.Replace(" ", ".");
        }
        public static async Task<FileType> IdentifyFileType(string url)
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { MaxRequestContentBufferSize = 104857600 }))
            {
                FileType fileType = new();
                try
                {
                    var rangeRequest = new HttpRequestMessage(HttpMethod.Get, url);
                    rangeRequest.Headers.Range = new RangeHeaderValue(0, 7);

                    HttpResponseMessage response = await httpClient.SendAsync(rangeRequest, HttpCompletionOption.ResponseHeadersRead);


                    if (response.IsSuccessStatusCode)
                    {
                        var contentHeaders = response.Content.Headers;
                        var firstBytes = response.Content.ReadAsByteArrayAsync().Result;

                        string signatureHex = BitConverter.ToString(firstBytes).Replace("-", "").ToUpper().Substring(0, 8);

                        if (FileTypeSignatures.ContainsKey(signatureHex))
                        {
                            fileType.fileExtension = FileTypeSignatures[signatureHex];
                        }
                        if (contentHeaders.TryGetValues("Content-Type", out IEnumerable<string> contentTypeValues))
                        {
                            string pattern = @"^[a-zA-Z0-9!#$&+.\-^_]+/[a-zA-Z0-9!#$&+.\-^_]+$";
                            if (Regex.IsMatch(contentTypeValues?.FirstOrDefault(), pattern))
                            {
                                fileType.contentType = contentTypeValues?.FirstOrDefault();

                            }
                            
                            fileType.contentType = string.IsNullOrEmpty(fileType.contentType)? (FileContentTypes.ContainsKey(fileType.fileExtension) ? FileContentTypes[fileType.fileExtension] : "unknown/unknown"): "unknown/unknown";

                            return fileType;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la requête HTTP. Statut : {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erreur de requête HTTP : {ex.Message}");
                }

                return new FileType{ fileExtension = ".unknown",contentType="" }; // If the file type can't be determined.
            }
        }

        static Dictionary<string, string> FileTypeSignatures = new Dictionary<string, string>
        {
            { "25504446", "pdf" },   // PDF file signature
            { "504B0304", "zip" },   // ZIP file signature
            { "52657420", "txt" },   // Plain Text file signature
            { "47494638", "gif" },   // GIF file signature
            { "89504E47", "png" },   // PNG file signature
            { "52617221", "rar" },
            { "FFD8FFE0", "jpg" },   // JPEG (start of image)
            { "FFD8FFE1", "jpg" },   // JPEG (start of image)
            { "FFD8FFE8", "jpg" },   // JPEG (start of image)
            { "FFD8FFE2", "jpg" },   // JPEG (start of image)
            { "1F8B0808", "gz" },    // GZIP compressed file signature
            { "49443303", "mp3" },   // MP3 file signature
            { "52494646", "avi" },   // AVI file signature
            { "1A45DFA3", "mkv" },   // Matroska (MKV) file signature
            { "00000020", "mp4" },   // MP4 file signature
            { "57415645", "wav" },   // WAV file signature
            { "504B0708", "7z" },    // 7-Zip compressed file signature
            { "504B0306", "xlsx" },  // Microsoft Excel (XLSX) file signature
            { "504B0507", "docx" },  // Microsoft Word (DOCX) file signature
            { "377ABCAF271C", "7z" }, // Old 7-Zip file signature
            { "7B5C727466", "rtf" }, // Rich Text Format (RTF) file signature
            { "4D5A9000", "exe" },   // Windows Executable (PE) file signature
            { "89504E470D0A1A0A", "png" }, // PNG file signature
            { "D0CF11E0A1B11AE1", "doc" }, // Microsoft Word (DOC) file signature
            { "504B030414000100", "jar" }, // Java Archive (JAR) file signature
            { "4D54686400000006", "mid" }, // MIDI file signature
            { "424D36B402000000", "bmp" }, // Bitmap (BMP) file signature
            { "255044462D312E", "pdf" },   // PDF file signature
            { "667479704D534E56", "mp4" }, // MPEG-4 (MP4) file signature
            { "4F67675300020000", "ogg" }, // Ogg Vorbis audio file signature
            { "AC9EBD8F00000000", "qxd" }, // QuarkXPress document signature
            { "D7CDC69A", "wmv" }, // Windows Media Video (WMV) file signature
            { "6D6F6F76", "mov" }, // QuickTime Movie (MOV) file signature
            { "252150532D41646F6265", "eps" }, // Encapsulated PostScript (EPS) file signature
        };
        static Dictionary<string, string> FileContentTypes = new Dictionary<string, string> //methode de sauvageon pour l'instant
        {
            { "txt", "text/plain" },
            { "html", "text/html" },
            { "htm", "text/html" },
            { "xml", "text/xml" },
            { "jpeg", "image/jpeg" },
            { "jpg", "image/jpeg" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "mp3", "audio/mpeg" },
            { "mp4", "video/mp4" },
            { "pdf", "application/pdf" },
            { "doc", "application/msword" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "xls", "application/vnd.ms-excel" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "zip", "application/zip" },
            { "rar", "application/x-rar-compressed" },
            { "json", "application/json" },
            { "js", "application/javascript" }
        };

    }

}
