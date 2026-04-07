using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using advertisementsSystem.Mvc.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MVCAdvertisment.Models;

namespace advertisementsSystem.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //Visa vyn för att skapa en annons
    public IActionResult createAdvertisment()
    {
        
        return View();
    }

    //Hämte en prenumerant från API:et och visa den i vyn
    public async Task<IActionResult> getSubscriber(int subNumber)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5285/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync($"api/Subscriber/{subNumber}");
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("API call successful");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var subscriber = JsonConvert.DeserializeObject<SubscriberDetails>(apiResponse);
            return View("createAdvertisment", subscriber);
        }
        Console.WriteLine("API call failed");

        return View("createAdvertisment");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
