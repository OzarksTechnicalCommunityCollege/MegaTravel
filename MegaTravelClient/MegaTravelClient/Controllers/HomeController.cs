using Microsoft.AspNetCore.Mvc;
using MegaTravelClient.Models;
using Newtonsoft.Json;
using MegaTravelClient.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MegaTravelClient.Controllers
{

    public class HomeController : Controller
    {
        public IConfiguration _configuration;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel userData)
        {
            //check to see if there are any errors before we proceed
            if (ModelState.IsValid)
            {
                ProcessForm(userData);
                return View("Views/Home/Login.cshtml");
            }
            else
            {
                return View();

            }
            //return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> GetUsers()
        {
            //List<GetUsersResponseModel> userList = new List<GetUsersResponseModel>();

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
            if (userData != null)
            {
                //depending on whether the user has chosen to login as a user or an agent, we'll handle these differently
                if (userData.UserType == "User")
                {
                    //use the userData recieved from the form to make an API call that checks the login data of the user

                    //hash the user's password which has been collected as plain text
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    //update the object so we're not sending plain text password data to our API
                    userData.Password = hashedPassword;

                    //call the API with the user data
                    try
                    {
                        LoginResponseModel ResponseModel = null;
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        Console.WriteLine(strSerializedData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginUser, false, string.Empty).ConfigureAwait(true);
                        ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                        if (ResponseModel == null)
                        {
                            //error with the API that caused a null return. what happened?  Needs handled

                        }
                        else if (ResponseModel.Status == false)
                        {
                            //did not log in. Maybe a wrong username or password
                            ViewBag.ErrorMessage = "Email or Password Incorrect";
                            return View("Views/Home/Login.cshtml");
                        }
                        else
                        {
                            //everything is OK
                            //the user should now be considered authenticated
                            //create claims details based on the user information
                            var claims = new[] {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("UserId", ResponseModel.UserID.ToString())
                             };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(10),
                                signingCredentials: signIn);

                            //Take the user to the UserDashboard
                            return View("Views/User/Index.cshtml", ResponseModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ProcessLogin API " + ex.Message);
                    }
                }
                else if (userData.UserType == "Agent")
                {
                    //use the userData recieved from the form to make an API call that checks the login data of the agent

                    //hash the user's password which has been collected as plain text
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    //update the object so we're not sending plain text password data to our API
                    userData.Password = hashedPassword;

                    //call the API with the user data
                    try
                    {
                        LoginResponseModel ResponseModel = null;
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        Console.WriteLine(strSerializedData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginAgent, false, string.Empty).ConfigureAwait(true);
                        ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                        if (ResponseModel == null)
                        {
                            //error with the API that caused a null return. what happened?  Needs handled

                        }
                        else if (ResponseModel.Status == false)
                        {
                            //did not log in. Maybe a wrong username or password
                            ViewBag.ErrorMessage = "Email or Password Incorrect";
                            return View("Views/Home/Login.cshtml");


                        }
                        else
                        {
                            //everything is OK
                            //the agent should now be considered authenticated
                            //Take the agent to the Agent Dashboard
                            return View("Views/Agent/Index.cshtml", ResponseModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ProcessLogin API " + ex.Message);
                    }
                }
            }
            return View();
        }
    }
}
