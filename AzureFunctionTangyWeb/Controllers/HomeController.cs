using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFunctionTangyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AzureFunctionTangyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly BlobServiceClient _blobServiceClient;

        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        //http://localhost:7045/api/OnSalesUploadWriteToQueue
        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest, IFormFile file)
        {
            /*
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:7045/api/OnSalesUploadWriteToQueue"),
                Content = new StringContent(JsonConvert.SerializeObject(salesRequest), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return View();
            */
            salesRequest.Id= Guid.NewGuid().ToString();

            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage httpResponse = await client.PostAsync("http://localhost:7045/api/OnSalesUploadWriteToQueue", content);
                string result= httpResponse.Content.ReadAsStringAsync().Result;


            }
            if (file != null)
            {
                var filename = salesRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("functionsalesreq");
                var blobclient = containerClient.GetBlobClient(filename);
                var httpHeaders = new BlobHttpHeaders
                {
                    ContentType=file.ContentType
                };
                await blobclient.UploadAsync(file.OpenReadStream(),httpHeaders);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
