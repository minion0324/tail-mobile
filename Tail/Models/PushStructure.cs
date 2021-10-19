namespace Tail.Models
{
    public class PushStructure
    {
        public int UserId { get; set; }
        public string PostId { get; set; }
        public int Badge { get; set; }
        public int Ptype { get; set; }
        public bool IsSessionOut { get; set; }
        public string NotificationId { get; set; }
    }
}
