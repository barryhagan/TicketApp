using System;
using System.Collections.Generic;

namespace TicketCore.Model
{
    public class ModelBase<TKey>
    {
        public TKey _id { get; set; }
        public Guid? external_id { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string url { get; set; }
        public List<string> tags { get; set; }
    }
}
