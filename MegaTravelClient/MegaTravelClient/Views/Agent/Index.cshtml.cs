using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MegaTravel.Pages
{
    public class AgentDashboardModel : PageModel
    {
        private readonly ILogger<AgentDashboardModel> _logger;

        public AgentDashboardModel(ILogger<AgentDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}