using System;
using System.Collections.Generic;

namespace Accounting.API
{
    public partial class EventrawBackup
    {
        public int Id { get; set; }
        public string InfoXml { get; set; }
        public string InfoJson { get; set; }

        public int? AcctStatusType { get; set; }
        public string UserName { get; set; }
        public DateTime? EventTimeStamp { get; set; }
        public long? AcctInputOctets { get; set; }
        public long? AcctOutputOctets { get; set; }
    }

    public partial class Eventraw
    {
        public int Id { get; set; }
        public string InfoXml { get; set; }
        public string InfoJson { get; set; }

        public int? AcctStatusType { get; set; }
        public string UserName { get; set; }
        public DateTime? EventTimeStamp { get; set; }
        public long? AcctInputOctets { get; set; }
        public long? AcctOutputOctets { get; set; }
    }
}
