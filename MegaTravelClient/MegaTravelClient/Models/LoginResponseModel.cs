namespace MegaTravelClient.Models
{
    public class LoginResponseModel
    {
        public int UserID { get; set; }
        public string AccountType { get; set; } = null;
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public string Authtoken { get; set; } = null;

        public User Data { get; set; } = null;

        public Agent AgentData { get; set; } = null;

    }
}
