using System;
using System.Collections.Generic;
using System.Text;

namespace TicketCore.Model
{
    public class SearchHit
    {
        public string DocId { get; set; }
        public string DocType { get; set; }
        public float Score { get; set; }
    }
}
