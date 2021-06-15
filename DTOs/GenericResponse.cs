namespace ZooMag.DTOs
{
    public class GenericResponse<T>
    {
        public T Payload { get; set; }
        public int Count { get; set; }
    }
}