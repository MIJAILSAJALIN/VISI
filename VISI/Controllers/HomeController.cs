using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
//using System.Web.Mvc;
using VISI.Models;

namespace VISI.Controllers
{

    

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        
        [AllowAnonymous]
        public IActionResult NoEncontrado()
        {
            return View("NoEncontrado");
        }

        [HttpPost]
        public IActionResult Validador(userFormValid userFormValid) 

        {            
            //// código de pruebas para practicar las inyecciones de dependencia
            //UserActual userActual = userValid.isValid(userFormValid);
            //var aaa = userValid.ListUserValid;
            //if (userActual.UserValid) return RedirectToAction("Privacy");
            //else return View("Index");
            return View();
        }

        public IActionResult Index()
        {    
            return View();
        }

        public IActionResult Privacy()
        {
                return View();
            
        }
        [AllowAnonymous]
        public IActionResult Inicio()
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