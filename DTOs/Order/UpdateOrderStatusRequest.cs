namespace ZooMag.DTOs.Order
{
    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
    }
}