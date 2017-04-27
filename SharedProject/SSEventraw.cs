using System;

namespace SharedProject
{
    public partial class SSEventraw
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public long TotalBytesInOut { get; set; }
        public string MachineName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
