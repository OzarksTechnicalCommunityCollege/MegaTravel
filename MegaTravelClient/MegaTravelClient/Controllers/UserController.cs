using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;

namespace MegaTravelClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {

            return View();
        }
    }
}
