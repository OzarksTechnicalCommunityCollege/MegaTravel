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

        public async Task<IActionResult> MarkPaymentStatus(int tripID, int userID)
        {

            List<GetTripsForUserResponseModel> userList = new List<GetTripsForUserResponseModel>();

            GetTripsForUserResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string responseSet = await objService.PostRequest(strSerializedData, ConstantValues.SetTripPaymentStatus + "?tripID=" + tripID, false, string.Empty).ConfigureAwait(true);
                string responseGet = await objService.GetRequest(strSerializedData, ConstantValues.GetAllTripsByUser + "?userID=" + userID, false, string.Empty).ConfigureAwait(true);

                ResponseModel = JsonConvert.DeserializeObject<GetTripsForUserResponseModel>(responseGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MarkPaymentStatus API " + ex.Message);
            }
            //return the user to the specific view
            return View("Views/User/AllTripsView.cshtml", ResponseModel);
        }
    }
}
