using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using advertisementsSystem.Mvc.Models;
using Newtonsoft.Json;
using MVCAdvertisment.Models;
using System.Text;

namespace MVCAdvertisment.Controllers;
public class SubscriberController : Controller
{
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

            return View("~/Views/Ads/createAdvertisment.cshtml", viewModel);
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

        return View("~/Views/Ads/createAdvertisment.cshtml", failedViewModel);
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
        return View("~/Views/Ads/createAdvertisment.cshtml", subNumber);
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
            return RedirectToAction("~/Views/Ads/createAdvertisment.cshtml", subscriber);
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