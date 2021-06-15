namespace ZooMag.Entities
{
    public class Box
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int BoxTypeId { get; set; }
        public int Number { get; set; }

        public virtual BoxType BoxType { get; set; }
    }
}
