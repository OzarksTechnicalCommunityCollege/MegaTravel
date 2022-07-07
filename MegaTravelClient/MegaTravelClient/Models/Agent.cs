namespace MegaTravelClient.Models
{
    public class Agent
    {
        public int AgentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string OfficeLocation { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
