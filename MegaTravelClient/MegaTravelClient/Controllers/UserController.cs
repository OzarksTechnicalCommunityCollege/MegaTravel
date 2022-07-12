using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using Microsoft.AspNetCore.Authorization;

namespace MegaTravelClient.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {

            return View();
        }
    }
}
