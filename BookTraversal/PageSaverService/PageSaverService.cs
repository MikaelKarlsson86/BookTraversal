using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookTraversal.PageSaverService
{
    public class PageSaverService
    {
        private readonly HttpClient _httpClient;
        private List<string> _savedPages = new List<string>();

        string localRootPath;
        const string rootUrl = "https://books.toscrape.com/";

        public PageSaverService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(rootUrl)
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _savedPages = new List<string>();

            localRootPath = System.Reflection.Assembly.GetExecutingAssembly().Location + "\\..\\Books\\";
        }

        public async Task SavePage(string fileName = "index.html")
        {
            var activePageUrl = rootUrl + fileName;

            if (_savedPages.Contains(activePageUrl))
                return;

            Console.WriteLine($"Processing page {activePageUrl}");

            _savedPages.Add(activePageUrl);

            var file = File.Create(localRootPath + "\\" + fileName);
            file.Write(await _httpClient.GetByteArrayAsync(activePageUrl));
            file.Close();

            var document = new HtmlWeb().Load(activePageUrl);

            SaveCSSForPage(document);
            SaveImagesForPage(document, localRootPath);

            var links = document.DocumentNode.SelectNodes("//a").Where(x => x.Attributes["href"].Value != "index.html" && !_savedPages.Contains(rootUrl + x.Attributes["href"].Value));

            foreach (var link in links)
            {
                var href = link.Attributes["href"].Value;
                var hrefWithoutSteps = string.Join('/', href.Split('/').Where(x => x != ".."));
                var numberOfstepsUp = href.Split('/').Count(x => x == "..");

                var activeFileNameParts = activePageUrl.Substring(27).Split('/').SkipLast(1);
                var newFileNameBase = string.Join('/', activeFileNameParts.SkipLast(numberOfstepsUp));
                var newFileName = newFileNameBase + (!string.IsNullOrWhiteSpace(newFileNameBase) ? "/" : "") + hrefWithoutSteps;

                Directory.CreateDirectory(localRootPath + "\\" + newFileName[..^10]);

                await SavePage(newFileName);
            }
        }

        public void SaveImagesForPage(HtmlDocument document, string localPageFolder)
        {
            var images = document.DocumentNode.SelectNodes("//img");

            Console.Write($"Saving images for {document.DocumentNode.OriginalName}...");

            if(images != null && images.Any())
            {
                foreach (var image in images)
                {
                    var imageUrl = image.Attributes["src"].Value;
                    var imageFolder = string.Join('\\', image.Attributes["src"].Value.Split('/').SkipLast(1).SkipWhile(x => x == ".."));
                    Directory.CreateDirectory(localPageFolder + "\\" + imageFolder);
                    var imgFile = File.Create(localPageFolder + "\\" + imageFolder + "\\" + imageUrl.Split('/').Last());
                    imgFile.Write(_httpClient.GetByteArrayAsync(imageUrl).Result);
                    imgFile.Close();
                    Console.WriteLine($"Writing image file {imgFile.Name}");
                }

                Console.WriteLine($"{images.Count} images found");
                return;
            }

            Console.WriteLine("No images found");
        }

        public void SaveCSSForPage(HtmlDocument document)
        {
            var cssFiles = document.DocumentNode.SelectNodes("//head/link").Where(x => x.Attributes["href"].Value.EndsWith(".css"));

            if (cssFiles != null && cssFiles.Any())
            {
                foreach (var css in cssFiles)
                {
                    var cssUrl = css.Attributes["href"].Value;
                    var cssFolder = string.Join('\\', css.Attributes["href"].Value.Split('/').SkipLast(1).SkipWhile(x => x == ".."));
                    Directory.CreateDirectory(localRootPath + "\\" + cssFolder);
                    var cssFile = File.Create(localRootPath + "\\" + cssFolder + "\\" + cssUrl.Split('/').Last());
                    cssFile.Write(_httpClient.GetByteArrayAsync(cssUrl).Result);
                    cssFile.Close();
                    Console.WriteLine($"Writing css file {cssFile.Name}");
                }

                Console.WriteLine($"{cssFiles.Count()} css files found");
                return;
            }
        }
    }
}
