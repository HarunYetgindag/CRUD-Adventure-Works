using CRUD.AWDB.UI.Dtos;
using CRUD.AWDB.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CRUD.AWDB.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        HttpClientHandler _clientHandler = new HttpClientHandler();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Add()
        {
            return PartialView("_PersonAddPartial");
        }

        public async Task<IActionResult> Update(int Id)
        {
            var updateModel = new UpdateDto();


            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7192/AdventureWorks/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updateModel = JsonConvert.DeserializeObject<UpdateDto>(apiResponse);
                    return PartialView("_PersonUpdatePartial", updateModel);
                }

            }


        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}