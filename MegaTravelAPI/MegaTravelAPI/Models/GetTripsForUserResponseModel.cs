using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class GetTripsForUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } 
        public List<Trip> tripList { get; set; }
    }
}
