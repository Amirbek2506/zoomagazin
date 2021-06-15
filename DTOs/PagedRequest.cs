namespace ZooMag.DTOs
{
    public class PagedRequest
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
    }
}