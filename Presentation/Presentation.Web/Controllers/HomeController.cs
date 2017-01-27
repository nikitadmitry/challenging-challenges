using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var test = User;
            var test1 = HttpContext.User;
            var user = userManager.GetUserAsync(HttpContext.User);

            return View();
        }

        public IActionResult NavMenu()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
