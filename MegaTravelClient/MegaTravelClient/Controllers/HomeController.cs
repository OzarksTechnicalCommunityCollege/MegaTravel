using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using Newtonsoft.Json;
using MegaTravelClient.Utility;

namespace MegaTravelClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> GetUsers()
        {
            List<GetUsersResponseModel> userList = new List<GetUsersResponseModel>();

            GetUsersResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllUsersAPI, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetUsersResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllUsers API " + ex.Message);
            }
            return View(ResponseModel);

        }

        /// <summary>
        /// Method that handles the registration form 
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProcessForm(RegistrationModel userData)
        {
            //use the userData recieved from the form to make an API call that adds
            //the new user to the database

            //hash the user's password which has been collected as plain text
            string hashedPassword = Helper.EncryptCredentials(userData.Password);

            //update the object so this is what is sent to the database
            userData.Password = hashedPassword;

            //call the API with the user data
            try
            {
                RegistrationResponseModel ResponseModel = null;
                var strSerializedData = JsonConvert.SerializeObject(userData);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.RegisterUser, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<RegistrationResponseModel>(response);

                if(ResponseModel == null)
                {
                    //error with the API that caused a null return

                }
                else if(ResponseModel.Status == false)
                {
                    //an error occured

                }
                else
                {
                    //everything is OK - return
                    return View(ResponseModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration API " + ex.Message);
            }
            return View();
        }

        /// <summary>
        /// Method to handle the login for the user or the agent
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProcessLogin(LoginModel userData)
        {
            //use the userData recieved from the form to make an API call that checks the login data of the user

            //hash the user's password which has been collected as plain text
            string hashedPassword = Helper.EncryptCredentials(userData.Password);

            //update the object so we're not sending plain text password data to our API
            userData.Password = hashedPassword;

            //Set this as a user login
            userData.UserType = "User";

            //call the API with the user data
            try
            {
                LoginResponseModel ResponseModel = null;
                var strSerializedData = JsonConvert.SerializeObject(userData);
                Console.WriteLine(strSerializedData);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginAPI, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                if (ResponseModel == null)
                {
                    //error with the API that caused a null return. what happened?  Needs handled

                }
                else if (ResponseModel.Status == false)
                {
                    //an error occured, what happened?  Needs handled

                }
                else
                {
                    //everything is OK
                    //the user should now be considered authenticated


                    //Take the user to the UserDashboard
                    return View("Views/User/Index.cshtml", ResponseModel);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ProcessLogin API " + ex.Message);
            }

            return View();
        }
    }
}
