namespace ZooMag.DTOs.Shop
{
    public class CreateShopRequest
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Graphic { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}