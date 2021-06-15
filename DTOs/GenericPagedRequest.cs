namespace ZooMag.DTOs
{
    public class GenericPagedRequest<T> : PagedRequest
    {
        public T Query { get; set; }

    }
}