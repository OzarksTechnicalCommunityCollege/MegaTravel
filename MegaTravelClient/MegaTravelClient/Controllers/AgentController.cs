using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using MegaTravelClient.Utility;
using Newtonsoft.Json;

namespace MegaTravelClient.Controllers
{
    public class AgentController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {
            return View();
        }


        public async Task<IActionResult> AllTripsView([FromQuery(Name = "agentID")] int agentID)
        {

            List<GetTripsResponseModel> userList = new List<GetTripsResponseModel>();

            GetTripsResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllTripsForAgent + "?agentID=" + agentID, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetTripsResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AllTripsView API " + ex.Message);
            }
            return View(ResponseModel);
        }
    }
}
