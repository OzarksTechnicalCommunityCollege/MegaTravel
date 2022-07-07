using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MegaTravel.Pages
{
    public class ProcessFormModel : PageModel
    {
        private readonly ILogger<ProcessFormModel> _logger;

        public ProcessFormModel(ILogger<ProcessFormModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}