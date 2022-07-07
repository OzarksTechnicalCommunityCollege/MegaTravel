
namespace MegaTravelClient.Models
{
    public class RegistrationResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public User Data { get; set; } = null;
    }
}
