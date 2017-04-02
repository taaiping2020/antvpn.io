namespace Server.API.Models
{
    public class ServerProtocal
    {
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public int ProtocalId { get; set; }
        public virtual Protocal Protocal { get; set; }
    }
}