using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MegaTravelClient.Models;

namespace MegaTravel.Pages
{
    public class AgentDashboardModel : PageModel
    {
        protected LoginResponseModel _myObject;

        private readonly ILogger<AgentDashboardModel> _logger;

        public AgentDashboardModel(ILogger<AgentDashboardModel> logger)
        {
            _logger = logger;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void OnGet()
        {

        }
    }
}