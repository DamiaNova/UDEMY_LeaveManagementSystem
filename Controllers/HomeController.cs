using System.Diagnostics;
using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Izvedena klasa (apstraktna bazna klasa: Controller)
    /// Može renderati više View-stranica
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Privatan field korišten zbog DIP-a (Dependency Inversion Principle)
        /// HomeController označava konkretnu klasu kojoj je loger namijenjen
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Custom-konstruktor MVC Controllera
        /// </summary>
        public HomeController(ILogger<HomeController> logger)
        {
            //Injektiranje putem dependency injectiona:
            _logger = logger;
        }

        /// <summary>
        /// Return tip 'IActionResult' se postavlja nekim Action-metodama
        /// Ovo je definirano kao defaultni Action u Routing mehanizmu (Program.cs)
        /// </summary>
        public IActionResult Index()
        {
            //Ovdje se definira bussiness logika
            // ...
            //Controller uvijek na kraju mora vratiti View-stranicu:
            return View();
        }

        /// <summary>
        /// Return tip 'IActionResult' se postavlja nekim Action-metodama
        /// </summary>
        public IActionResult Privacy()
        {
            //Ovdje se definira bussiness logika
            // ...
            //Controller uvijek na kraju mora vratiti View-stranicu:
            return View();
        }

        /// <summary>
        /// Action koji vraća Error View
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //Stvaranje novog objekta ViewModel klase ErrorViewModel.cs
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            //Vraćanje Viewa zajedno sa objektom koji sadrži podatke za View
            return View(model);
        }
    }
}
