using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using Microsoft.AspNetCore.Authorization;
using MegaTravelClient.Utility;
using Newtonsoft.Json;

namespace MegaTravelClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {

            return View();
        }

        public async Task<IActionResult> AllTripsView([FromQuery(Name = "userID")] int userID)
        {

            List<GetTripsForUserResponseModel> userList = new List<GetTripsForUserResponseModel>();

            GetTripsForUserResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllTripsByUser + "?userID=" + userID, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetTripsForUserResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AllTripsByUser API " + ex.Message);
            }
            return View(ResponseModel);
        }
    }
}
