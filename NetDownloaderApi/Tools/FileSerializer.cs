using System;
using System.Net.Http.Headers;

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

                        if (!Path.HasExtension(fileName))
                        {
                            MediaTypeHeaderValue contentType = contentHeaders.ContentType;
                            if (contentType.MediaType != null)
                            {
                                string fileExtension = contentType.MediaType.Split('/').LastOrDefault();

                                if (!string.IsNullOrEmpty(fileExtension))
                                {
                                    return fileName + "." + fileExtension; // Vous pouvez personnaliser la logique de nommage ici
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
        public static async Task<string> IdentifyFileType(string url)
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { MaxRequestContentBufferSize = 104857600 }))
            {

                try
                {
                    // Create an HTTP request to retrieve the first few bytes (e.g., 0-7).
                    var rangeRequest = new HttpRequestMessage(HttpMethod.Get, url);
                    rangeRequest.Headers.Range = new RangeHeaderValue(0, 7); // Adjust the range as needed.

                    // Send the request and get the response.
                    //HttpResponseMessage response = httpClient.SendAsync(rangeRequest, HttpCompletionOption.ResponseHeadersRead).Result; //worked, but not async
                    HttpResponseMessage response = await httpClient.SendAsync(rangeRequest, HttpCompletionOption.ResponseHeadersRead);


                    if (response.IsSuccessStatusCode)
                    {
                        var contentHeaders = response.Content.Headers;
                        var firstBytes = response.Content.ReadAsByteArrayAsync().Result;

                        string signatureHex = BitConverter.ToString(firstBytes).Replace("-", "").ToUpper().Substring(0, 8);

                        if (FileTypeSignatures.ContainsKey(signatureHex))
                        {
                            return FileTypeSignatures[signatureHex];
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

                return "unknown"; // If the file type can't be determined.


            }
        }

        static byte[] ReadFirstBytes(byte[] bytes, int count)
        {
            return bytes.Take(count).ToArray();
        }
        static Dictionary<string, string> FileTypeSignatures = new Dictionary<string, string>
        {
            { "25504446", "pdf" },   // PDF file signature
            { "504B0304", "zip" },   // ZIP file signature
            { "52657420", "txt" },   // Plain Text file signature
            { "47494638", "gif" },   // GIF file signature
            { "89504E47", "png" },   // PNG file signature
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
    }

}
