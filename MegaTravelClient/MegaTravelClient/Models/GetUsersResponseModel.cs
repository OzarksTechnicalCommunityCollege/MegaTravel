using System.Collections;

namespace MegaTravelClient.Models
{
    public class GetUsersResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public List<User> userList { get; set; } = null;



    }
}
