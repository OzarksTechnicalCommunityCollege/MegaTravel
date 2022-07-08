using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MegaTravel.Pages
{
    public class AllTripsViewModel : PageModel
    {
        private readonly ILogger<AllTripsViewModel> _logger;

        public AllTripsViewModel(ILogger<AllTripsViewModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}