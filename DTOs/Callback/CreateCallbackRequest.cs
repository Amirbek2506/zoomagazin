namespace ZooMag.DTOs.Callback
{
    public class CreateCallbackRequest
    {
        public string PhoneNumber { get; set; }
        public string FromHour { get; set; }
        public string TillHour { get; set; }
        public string Name { get; set; }
    }
}
