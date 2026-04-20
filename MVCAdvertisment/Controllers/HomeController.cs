using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using advertisementsSystem.Mvc.Models;
using Newtonsoft.Json;
using MVCAdvertisment.Models;
using System.Text;

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
    [HttpGet]
    public IActionResult createAdvertisment()
    {
        var viewModel = new CreateAdvertismentViewModel
        {
            Subscriber = null,
            Ad = new AdsDetails(),
            Annonsorer = new AnnonsorerDetails(),
            ShowSubscriberForm = false,
            ShowCompanyForm = false,
            ShowAdForm = false
        };
        
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult createAdvertisment(CreateAdvertismentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("createAdvertisment", new CreateAdvertismentViewModel { Ad = vm.Ad });
        }

        AdsMethods methods = new AdsMethods();
        Console.WriteLine("Ad title: " + vm.Ad.AdTitle);
        Console.WriteLine("Ad price: " + vm.Ad.AdPrice);
        Console.WriteLine("AdvId: " + vm.Ad.AdvId);
        int rowsInserted = methods.InsertAd(vm.Ad);
        if (rowsInserted > 0)
        {
            var viewModel = new CreateAdvertismentViewModel
            {
                Subscriber = null,
                Ad = vm.Ad,
                Annonsorer = new AnnonsorerDetails(),
                ShowSubscriberForm = false,
                ShowCompanyForm = false,
                ShowAdForm = true
            };
            return View("createAdvertisment", viewModel);
        }
        Console.WriteLine("Failed to insert ad");

        var failedViewModel = new CreateAdvertismentViewModel
        {
            Subscriber = null,
            Ad = vm.Ad,
            Annonsorer = new AnnonsorerDetails(),
            ShowSubscriberForm = false,
            ShowCompanyForm = false,
            ShowAdForm = false,
        };

        return View("createAdvertisment", failedViewModel);
    }

    [HttpPost]
    public IActionResult InsertOrFetchAdvertiser(CreateAdvertismentViewModel vm)
    {
        ModelState.Remove("Ad");
        ModelState.Remove("Subscriber");    

        AnnonsorerDetails annonsor = vm.Annonsorer;

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state is invalid");
            var invalidViewModel = new CreateAdvertismentViewModel
            {
                Subscriber = null,
                Ad = new AdsDetails{ AdPrice = 40 },
                Annonsorer = annonsor,
                ShowSubscriberForm = false,
                ShowCompanyForm = true,
                ShowAdForm = true
            };
            Console.WriteLine("VIEWMODEL Ad.AdvId before return: " + invalidViewModel.Ad.AdvId);
            return View("createAdvertisment", invalidViewModel);
        }
    
        AnnonsorerMethods methods = new AnnonsorerMethods();
        AnnonsorerDetails existingAnnonsor = methods.GetAdvertiserByAdvNumber(annonsor.AdvNumber);
        if (existingAnnonsor != null)
        {
            Console.WriteLine("Advertiser already exists with this number");
            Console.WriteLine("AdvId: " + existingAnnonsor.AdvId);
            var viewModel = new CreateAdvertismentViewModel
            {
                Subscriber = null,
                Ad = new AdsDetails 
                { 
                    AdPrice = 40, 
                    AdvId = existingAnnonsor.AdvId
                },
                Annonsorer = existingAnnonsor,
                ShowSubscriberForm = false,
                ShowCompanyForm = true,
                ShowAdForm = true
            };
            Console.WriteLine("VIEWMODEL Ad.AdvId before return: " + viewModel.Ad.AdvId);
            return View("createAdvertisment", viewModel);
        }

        int rows = methods.InsertAdvertiser(annonsor);
        AnnonsorerDetails insertedAnnonsor = methods.GetAdvertiserByAdvNumber(annonsor.AdvNumber);
        if (rows != 0)
        {
            Console.WriteLine("Advertiser inserted successfully");
            var viewModel = new CreateAdvertismentViewModel
            {
                Subscriber = null,
                Ad = new AdsDetails
                { 
                    AdPrice = 40, 
                    AdvId = insertedAnnonsor.AdvId 
                },
                Annonsorer = insertedAnnonsor,
                ShowSubscriberForm = false,
                ShowCompanyForm = true,
                ShowAdForm = true
            };
            Console.WriteLine("VIEWMODEL Ad.AdvId before return: " + viewModel.Ad.AdvId);
            return View("createAdvertisment", viewModel);
        }
        Console.WriteLine("Failed to insert advertiser");

        var failedViewModel = new CreateAdvertismentViewModel
        {
            Subscriber = null,
            Ad = new AdsDetails{ AdPrice = 40 },
            Annonsorer = annonsor,
            ShowSubscriberForm = false,
            ShowCompanyForm = false,
            ShowAdForm = false
        };
        Console.WriteLine("VIEWMODEL Ad.AdvId before return: " + failedViewModel.Ad.AdvId);
        return View("createAdvertisment", failedViewModel);
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
            
            var viewModel = new CreateAdvertismentViewModel
            {
                Subscriber = subscriber,
                Ad = new AdsDetails(),
                Annonsorer = new AnnonsorerDetails(),
                ShowSubscriberForm = true,
                ShowCompanyForm = false,
                ShowAdForm = true
            };

            return View("createAdvertisment", viewModel);
        }
        Console.WriteLine("API call failed");
        
        var failedViewModel = new CreateAdvertismentViewModel
        {
            Subscriber = null,
            Ad = new AdsDetails(),
            Annonsorer = new AnnonsorerDetails(),
            ShowSubscriberForm = false,
            ShowCompanyForm = false,
            ShowAdForm = true
        };  

        return View("createAdvertisment", failedViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditSubscriber(string subNumber)
    {
        if (string.IsNullOrWhiteSpace(subNumber))
        {
            return Content("subNumber saknas");
        }

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
            return View("EditSubscriber", subscriber);
        }
        return View("selectAnnonsorer", subNumber);
    }

    [HttpPost]
    public async Task<IActionResult> EditSubscriber(SubscriberDetails subscriber)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5285/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var json = JsonConvert.SerializeObject(subscriber);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PutAsync($"api/Subscriber/{subscriber.SubscriptionNumber}", content);
        string apiResponse = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();



        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("API call successful");
            return RedirectToAction("selectAnnonsorer", subscriber);
        }
        Console.WriteLine("API call failed");
        
        return View("EditSubscriber", subscriber.SubscriptionNumber);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
