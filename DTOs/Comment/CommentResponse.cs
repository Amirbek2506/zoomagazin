using System;

namespace ZooMag.DTOs.Comment
{
    public class CommentResponse
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public int Rating { get; set; }
    }
}