namespace ZooMag.Entities
{
    public class ServImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsBannerImage { get; set; }
        public int AdditionalServId { get; set; }
        public virtual AdditionalServ AdditionalServes{ get; set; }
    }
}