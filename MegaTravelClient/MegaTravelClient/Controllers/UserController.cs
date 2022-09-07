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


        public async Task<ActionResult> Edit(int id)
        {
            //call the API with the user id to get the user data
            try
            {
                User currentUser = null;

                GetUsersResponseModel ResponseModel = null;

                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllUsersAPI, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetUsersResponseModel>(response);

                //loop through the list of users returned to find the one we want
                foreach(User user in ResponseModel.userList)
                {
                    if(user.UserId == id)
                    {
                        //this is the one we are looking for
                        currentUser = user;
                        break;
                    }
                }

                //return the current user data to the view
                return View(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get User Data " + ex.Message);
            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Edit(User updatedUser)
        {
            //update the data in the database
            try
            {
                LoginResponseModel ResponseModel = null;
                var strSerializedData = JsonConvert.SerializeObject(updatedUser);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.UpdateUser, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);

                if (ResponseModel == null)
                {
                    //error with the API that caused a null return

                }
                else if (ResponseModel.Status == false)
                {
                    //an error occured

                }
                else
                {
                    //Take the user to the UserDashboard
                    return View("Views/User/Index.cshtml", ResponseModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration API " + ex.Message);
            }
            //return the user to the dashboard
            return View();
        }

    }

   
}
