using System;

namespace TicketCore.Model
{
    public class User : ModelBase<int>
    {
        public string name { get; set; }
        public string alias { get; set; }
        public bool active { get; set; }
        public bool verified { get; set; }
        public bool shared { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public DateTimeOffset last_login_at { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string signature { get; set; }
        public int organization_id { get; set; }
        public bool suspended { get; set; }
        public string role { get; set; }
    }
}
