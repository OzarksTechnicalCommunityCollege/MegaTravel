using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using MegaTravelClient.Utility;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Web;

namespace MegaTravelClient.Controllers
{
   // [Authorize]
    public class AgentController : Controller
    {
        public static List<TripData> staticTripList;

        public IActionResult Index(LoginResponseModel userData)
        {
            return View();
        }


        public async Task<IActionResult> AllTripsView([FromQuery(Name = "agentID")] int agentID)
        {

            GetTripsResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllTripsForAgent + "?agentID=" + agentID, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetTripsResponseModel>(response);

                staticTripList = new List<TripData>();

                foreach (var item in ResponseModel.tripList)
                {
                    staticTripList.Add(item);
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("AllTripsView API " + ex.Message);
            }
            return View(ResponseModel);
        }

        public ActionResult DownloadTrips()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter objstreamwriter = new StreamWriter(stream);
                foreach (TripData trip in staticTripList)
                {
                    objstreamwriter.WriteLine(trip.AgentId + "," + trip.Agent + "," + trip.TripName);
                }   

                objstreamwriter.Flush();
                objstreamwriter.Close();
                return File(stream.ToArray(), "text/plain", "trips.csv");
            }
        }
    }
}
