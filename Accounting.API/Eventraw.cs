using System;
using System.Collections.Generic;

namespace Accounting.API
{
    public partial class Eventraw
    {
        public int Id { get; set; }
        public string InfoXml { get; set; }
        public string InfoJson { get; set; }
    }
}
