namespace ZooMag.DTOs.Shop
{
    public class UpdateShopRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Graphic { get; set; }
        public string PhoneNumber { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int CityId { get; set; }
    }
}