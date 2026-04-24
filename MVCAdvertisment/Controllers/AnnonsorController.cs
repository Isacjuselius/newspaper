using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using advertisementsSystem.Mvc.Models;
using Newtonsoft.Json;
using MVCAdvertisment.Models;
using System.Text;

namespace MVCAdvertisment.Controllers;
public class AnnonsorController : Controller
{
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
            return View("~/Views/Ads/createAdvertisment.cshtml", invalidViewModel);
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

            return View("~/Views/Ads/createAdvertisment.cshtml", viewModel);
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
            return View("~/Views/Ads/createAdvertisment.cshtml", viewModel);
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

        return View("~/Views/Ads/createAdvertisment.cshtml", failedViewModel);
    }
}