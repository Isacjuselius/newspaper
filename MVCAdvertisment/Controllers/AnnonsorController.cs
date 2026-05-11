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
        foreach (var error in ModelState)
        {
            Console.WriteLine($"Field: {error.Key}");

            foreach (var subError in error.Value.Errors)
            {
                Console.WriteLine($"Error: {subError.ErrorMessage}");
            }
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
        
        if (!ModelState.IsValid)
        {
            
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            foreach (var error in ModelState)
            {
                if (error.Value.Errors.Count > 0)  // Skriv bara ut fält som HAR fel
                {
                    Console.WriteLine($"Field: {error.Key}");
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"  Error: {subError.ErrorMessage}");
                        if (subError.Exception != null)
                            Console.WriteLine($"  Exception: {subError.Exception.Message}");
                    }
                }
            }
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