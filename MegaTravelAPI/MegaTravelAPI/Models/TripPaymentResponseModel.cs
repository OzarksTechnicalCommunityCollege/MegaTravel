using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models;

public class TripPaymentResponseModel
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public Trip trip { get; set; }
}
