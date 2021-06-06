namespace ZooMag.DTOs.Description
{
    public class CreateDescriptionRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ProductItemId { get; set; }
    }
}