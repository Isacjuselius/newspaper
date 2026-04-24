using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using advertisementsSystem.Mvc.Models;
using Newtonsoft.Json;
using MVCAdvertisment.Models;
using System.Text;

namespace MVCAdvertisment.Controllers;
public class AdsController : Controller
{
    [HttpGet]
    public IActionResult selectAds()
    {
        AdsMethods methods = new AdsMethods();
        List<AdsDetails> adsList = methods.GetAds();
        return View("selectAds", adsList);
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
        
        return View("~/Views/Ads/createAdvertisment.cshtml", viewModel);
    }

    [HttpPost]
    public IActionResult createAdvertisment(CreateAdvertismentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state is invalid");
            return View("~/Views/Ads/createAdvertisment.cshtml", new CreateAdvertismentViewModel { Ad = vm.Ad });
        }

        AnnonsorerMethods annonsorerMethods = new AnnonsorerMethods();

        //Om prenumerant information finns, försök hitta en annonsör med samma nummer om ingen sån finns skapa en ny annonsör
        if (vm.Subscriber != null)
        {
            AnnonsorerDetails? annonsor = annonsorerMethods.GetAdvertiserByAdvNumber(vm.Subscriber.SubscriptionNumber);

            if (annonsor == null)
            {
                AnnonsorerDetails newAnnonsor = new AnnonsorerDetails
                {
                    AdvName = vm.Subscriber.SubscriberFirstName + " " + vm.Subscriber.SubscriberLastName,
                    AdvNumber = vm.Subscriber.SubscriptionNumber,
                    AdvPhoneNumber = vm.Subscriber.SubscriberPhoneNumber,
                    AdvDeliveryAddress = vm.Subscriber.SubscriberDeliveryAddress,
                    AdvPostalCode = vm.Subscriber.SubscriberPostalCode,
                    AdvBillingAddress = vm.Subscriber.SubscriberDeliveryAddress,
                    AdvCity = vm.Subscriber.SubscriberCity
                };

                int rows = annonsorerMethods.InsertAdvertiser(newAnnonsor);

                if (rows == 0)
                {
                    Console.WriteLine("Failed to insert subscriber as advertiser");
                    return View("~/Views/Ads/createAdvertisment.cshtml", vm);
                }

                annonsor = annonsorerMethods.GetAdvertiserByAdvNumber(vm.Subscriber.SubscriptionNumber);

                if (annonsor == null)
                {
                    Console.WriteLine("Could not fetch subscriber advertiser after insert");
                    return View("~/Views/Ads/createAdvertisment.cshtml", vm);
                }
            }

            vm.Ad.AdvId = annonsor.AdvId;
            vm.Ad.AdPrice = 0;
        } else {
            
            Console.WriteLine("Subscriber information is missing");
            return View("~/Views/Ads/createAdvertisment.cshtml", new CreateAdvertismentViewModel { Ad = vm.Ad });
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
                Subscriber = vm.Subscriber,
                Ad = vm.Ad,
                Annonsorer = vm.Annonsorer,
                ShowSubscriberForm = false,
                ShowCompanyForm = false,
                ShowAdForm = true
            };
            return View("~/Views/Ads/selectAds.cshtml", viewModel);
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

        return View("~/Views/Ads/createAdvertisment.cshtml", failedViewModel);
    }
}