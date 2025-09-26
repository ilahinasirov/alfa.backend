using Application.Common.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Common.Implementation
{
    public class WordDocumentExtensionsService : IWordDocumentExtensionsService
    {
        private readonly HttpClient _httpClient;
        private const string GOTENBERG_URL = "http://65.21.157.163:3100";

        public WordDocumentExtensionsService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public async Task ReplacePlaceholdersAsync(MemoryStream docStream, Dictionary<string, string> replacements)
        {
            await Task.Run(() =>
            {
                docStream.Position = 0;

                using (var wordDoc = WordprocessingDocument.Open(docStream, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    foreach (var item in replacements)
                    {
                        ReplacePlaceholderInBody(body, $"#{item.Key}#", item.Value);
                    }

                    wordDoc.MainDocumentPart.Document.Save();
                }

                docStream.Position = 0;
            });
        }

        private void ReplacePlaceholderInBody(Body body, string placeholder, string replacement)
        {
            var texts = body.Descendants<Text>().ToList();

            for (int i = 0; i < texts.Count; i++)
            {
                var sb = new StringBuilder();
                int j = i;

                while (j < texts.Count && sb.Length < placeholder.Length)
                {
                    sb.Append(texts[j].Text);
                    j++;
                }

                string combinedText = sb.ToString();

                if (combinedText.Contains(placeholder))
                {
                    string newText = combinedText.Replace(placeholder, replacement);

                    texts[i].Text = newText;
                    for (int k = i + 1; k < j; k++)
                    {
                        texts[k].Text = string.Empty;
                    }

                    i = j - 1;
                }
            }
        }

        public async Task<byte[]> ConvertWordStreamToPdfAsync(MemoryStream docStream)
        {
            docStream.Position = 0;

            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(docStream.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            content.Add(fileContent, "files", "document.docx");

            var response = await _httpClient.PostAsync($"{GOTENBERG_URL}/forms/libreoffice/convert", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gotenberg conversion failed with status {response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
