using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MegaTravel.Pages
{
    public class ProcessLoginModel : PageModel
    {
        private readonly ILogger<ProcessLoginModel> _logger;

        public ProcessLoginModel(ILogger<ProcessLoginModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}