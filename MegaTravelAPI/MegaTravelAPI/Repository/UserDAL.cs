using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;
using Microsoft.Extensions.Logging;

namespace MegaTravelAPI.Data
{
    public class UserDAL : IUser
    {
        //context for the database connection
        private readonly MegaTravelContext context;

        private readonly IConfiguration _config;

        public UserDAL(MegaTravelContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }

        #region Get All Users Method
        /// <summary>
        /// Method that retrieves all users in the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();

            try
            {
                //query the database to get all of the users
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    userList.Add(new User()
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Street1 = user.Street1,
                        Street2 = user.Street2,
                        City = user.City,
                        State = user.State,
                        ZipCode = user.ZipCode,
                        Phone = user.Phone

                    });
                }

                if(userList.Count == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllUsers --- " + ex.Message);
                throw;
            }

            return userList;
        }

        #endregion

        #region Save User Record Method
        /// <summary>
        /// save the user registration record into database
        /// </summary>
        /// <param name="usermodel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SaveUserResponse> SaveUserRecord(RegistrationModel usermodel)
        {
            //set up a new object to hold our user information
            User objApplicationUser;

            //set up a response status object to hold the response
            SaveUserResponse res = new SaveUserResponse();

            try
            {
                //use the incoming information to populate our new user
                objApplicationUser = new User
                {
                    FirstName = usermodel.FirstName,
                    LastName = usermodel.LastName,
                    Email = usermodel.Email,
                    Street1 = usermodel.Street1,
                    Street2 = usermodel.Street2,
                    City = usermodel.City,
                    State = usermodel.State,
                    ZipCode = usermodel.ZipCode,
                    Phone = usermodel.Phone


                };

                //populate the Login information
                objApplicationUser.LoginInfo = new Login
                {
                    UserName = usermodel.Username,
                    Password = usermodel.Password,
                    UserType = "User"
                };

                //get the ID of the most recent user added to the DB
                var highestID = context.Users.Max(x => x.UserId);

                //set the user ID to be the next ID value
                objApplicationUser.UserId = highestID + 1;

                //save this user in the database
                using(var db = new MegaTravelContext(_config))
                {
                    db.Users.Add(objApplicationUser);
                    db.SaveChanges();
                }

                //update the login table with the username and password
                context.Database.ExecuteSqlRaw("[dbo].[AddLogin] @ID, @USERNAME, @PASSWORD, @TYPE",
                            new SqlParameter("@ID", objApplicationUser.UserId),
                            new SqlParameter("@USERNAME", objApplicationUser.LoginInfo.UserName),
                            new SqlParameter("@PASSWORD", objApplicationUser.LoginInfo.Password),
                            new SqlParameter("@TYPE", objApplicationUser.LoginInfo.UserType));


                //set the success to pass the data back
                res.StatusCode = 200;
                res.Message = "Save Successful";
                res.Status = true;
                res.Data = objApplicationUser;
                res.UserId = objApplicationUser.UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveUserRecord --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 0;
            }
            return res;
        }
        #endregion

        #region Login User Method
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                if(tokenData != null)
                {
                    //look for the user in the database
                    var query = context.Logins
                    .Where(x => x.UserName == tokenData.Username && x.Password == tokenData.Password)
                    .FirstOrDefault<Login>();

                    //if query has a result then we have a match
                    if(query != null)
                    {
                        res.Status = true;
                        res.StatusCode = 200;
                        res.Message = "Login Success";

                        //get the user data so we can send it back with
                        UserData user = await FindByName(query.UserName);
                        res.Data = new UserData
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Street1 = user.Street1,
                            Street2 = user.Street2,
                            City = user.City,
                            State = user.State,
                            ZipCode = user.ZipCode,
                            Phone = user.Phone
                        };
                        return res;
                    }
                    else
                    {
                        //the user wasn't found or wasn't a match
                        res.Status = false;
                        res.StatusCode = 500;
                        res.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginUser --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }

            return res;
        }
        #endregion

        #region Find User By Name Method

        public async Task<UserData> FindByName(string username)
        {
            UserData user = null;

            try
            {
                if (username != null)
                {
                    //query the database to find the user who has this username
                    var query = context.Users
                        .Where(x => x.LoginInfo.UserName == username)
                        .FirstOrDefault<User>();

                    if (query != null)
                    {
                        //set up the object so we can return it
                        user = new UserData
                        {
                            UserId = query.UserId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email,
                            Street1 = query.Street1,
                            Street2 = query.Street2,
                            City = query.City,
                            State = query.State,
                            ZipCode = query.ZipCode,
                            Phone = query.Phone
                        };
                    }
                    
                }

                return user;


            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByName --- " + ex.Message);
            }

            return user;
        }
        #endregion

        #region Get All Trips By User
        public List<Trip> GetTripsByUser(int userID)
        {
            //set up a list to hold the trips
            List<Trip> tripList = new List<Trip>();

            try
            {
                //query the database to get all of the users
                var trips = context.Trips
                    .Where(x => x.UserId == userID).ToList();

                if(trips != null)
                {
                    foreach (var trip in trips)
                    {
                        //get the agent associated with this trip
                        var agent = context.Agents
                            .Where(x => x.AgentId == trip.AgentId).FirstOrDefault();

                        Agent currentAgent = new Agent()
                        {
                            AgentId = agent.AgentId,
                            FirstName = agent.FirstName,
                            LastName = agent.LastName,
                            OfficeLocation = agent.OfficeLocation,
                            Phone = agent.Phone
                        };

                        //get the payment associated with this trip
                        var payment = context.TripPayment
                            .Where(x => x.TripId == trip.TripId).FirstOrDefault();

                        //if there is no payment record associated with this trip
                        //send back an empty object so we don't get a null reference
                        TripPayment paymentStatus = new TripPayment();

                        if (payment != null)
                        {
                            //if a payment record does exist, populate the object properties
                            paymentStatus.PaymentId = payment.PaymentId;
                            paymentStatus.TripId = payment.TripId;
                            paymentStatus.PaymentStatus = payment.PaymentStatus;
                           
                        }

                        tripList.Add(new Trip()
                        {
                            TripId = trip.TripId,
                            UserId = userID,
                            AgentId = trip.AgentId,
                            TripName = trip.TripName,
                            Location = trip.Location,
                            StartDate = trip.StartDate,
                            EndDate = trip.EndDate,
                            NumAdults = trip.NumAdults,
                            NumChildren = trip.NumChildren,
                            Agent = currentAgent,
                            PaymentStatus = paymentStatus,
                            Status = trip.Status
                        }); ;
                    }

                    if (tripList.Count == 0)
                    {
                        return null;
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTripsByUser --- " + ex.Message);
                throw;
            }

            return tripList;
        }
        #endregion

        #region Set Trip Payment Status Method
        /// <summary>
        /// Method that sets the payment status for the trip
        /// </summary>
        /// <param name="tripID"></param>
        /// <returns></returns>
        public async Task<TripPaymentResponseModel> SetTripPaymentStatus(int tripID)
        {
            //set up the response
            TripPaymentResponseModel response = new TripPaymentResponseModel();

            //object to hold the trip payment object information
            TripPayment payment = null;

            try
            {

                //update this information in the database
                using (var db = new MegaTravelContext(_config))
                {
                    //look for the existing record for this trip
                    var query = db.TripPayment.Where(x => x.TripId == tripID).FirstOrDefault();
                    Trip trip = db.Trips.Where(x => x.TripId == tripID).FirstOrDefault<Trip>();


                    if (query != null && trip != null)
                    {
                        
                        payment = new TripPayment()
                        {
                            TripId = query.TripId,
                            PaymentId = query.PaymentId,
                            PaymentStatus = true
                        };

                        //set the status to paid
                        query.PaymentStatus = true;
                        db.SaveChanges();

                        //connect the two objects
                        trip.PaymentStatus = payment;

                        //set the success to pass the data back
                        response.StatusCode = 200;
                        response.Message = "Payment Status Updated";
                        response.Status = true;
                        response.trip = trip;
                    }
                    else
                    {
                        //the payment record for this trip doesn't exist
                        response.StatusCode = 500;
                        response.Message = "The payment record for this trip doesn't exist";
                        response.Status = false;
                    }
                    
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("SetTripPaymentStatus --- " + ex.Message);
            }

            return response;
        }
        #endregion

        #region Cancel a Trip Method

        public async Task<TripPaymentResponseModel> CancelATrip(int tripID)
        {
            //set up the response
            TripPaymentResponseModel response = new TripPaymentResponseModel();

            try
            {
                //get the trip for this trip ID
                using (var db = new MegaTravelContext(_config))
                {
                    Trip trip = db.Trips.Where(x => x.TripId == tripID).FirstOrDefault<Trip>();

                    if (trip != null)
                    {
                        //get the trip payment information and connect the two objects
                        TripPayment payment = db.TripPayment.Where(x => x.TripId == tripID).FirstOrDefault<TripPayment>();
                        trip.PaymentStatus = payment;

                        //set the status to false to cancel the trip
                        trip.Status = false;
                        db.SaveChanges();

                        //set the success to pass the data back
                        response.StatusCode = 200;
                        response.Message = "Trip Canceled Successfully";
                        response.Status = true;
                        response.trip = trip;

                    }
                    else
                    {
                        //the record for this trip doesn't exist
                        response.StatusCode = 500;
                        response.Message = "The record for this trip doesn't exist";
                        response.Status = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CancelATrip --- " + ex.Message);
                throw;
            }

            return response;
            #endregion
        }

    }
}
