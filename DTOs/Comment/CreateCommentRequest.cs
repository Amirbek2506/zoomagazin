namespace ZooMag.DTOs.Comment
{
    public class CreateCommentRequest
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public int ProductItemId { get; set; }
    }
}