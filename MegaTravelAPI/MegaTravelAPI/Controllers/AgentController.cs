using Microsoft.AspNetCore.Mvc;
using MegaTravelAPI.Data;
using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace MegaTravelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AgentController : ControllerBase
    {
        // private readonly IUser repository;
        private readonly IAgent repository;

        //context for the database connection
        private readonly MegaTravelContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public AgentController(IConfiguration Config)
        {
            config = Config;
            context = new MegaTravelContext(config);
            repository = new AgentDAL(context);
            

        }

        [HttpPost("LoginAgent", Name = "LoginAgent")]
        [AllowAnonymous]
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                if (tokenData != null)
                {
                    //call the method that will check the user credentials
                    var loginResult = await repository.LoginAgent(tokenData).ConfigureAwait(true);

                    if (loginResult.StatusCode == 200)
                    {
                        //login check has succeeded

                        //query the database to get the information about our logged in user
                        var agent = await repository.FindByName(tokenData.Username).ConfigureAwait(true);

                        if (agent != null)
                        {
                            //generate the authentication token
                            var tokenString = GenerateJwtToken(tokenData);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AccountType = tokenData.UserType;
                            response.Authtoken = tokenString;
                            response.AgentData = new AgentData
                            {
                               
                                AgentID = agent.AgentId,
                                FirstName = agent.FirstName,
                                LastName = agent.LastName,
                                OfficeLocation = agent.OfficeLocation,
                                Phone = agent.Phone
                            };

                            return response;
                        }
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Status = false;
                        response.Message = "Login Failed";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAgemt --- " + ex.Message);
                response.StatusCode = 500;
                response.Status = false;
                response.Message = "Login Failed";
            }

            return response;
        }

        /// <summary>
        /// generate the token for registration
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJwtToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenAuthentication:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                //new Claim(JwtRegisteredClaimNames., userInfo.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            DateTime expiredDate = DateTime.UtcNow.AddDays(15);
            var token = new JwtSecurityToken(config["TokenAuthentication:Issuer"],
                config["TokenAuthentication:Issuer"],
                claims,
                expires: expiredDate,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
