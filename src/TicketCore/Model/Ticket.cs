using System;

namespace TicketCore.Model
{
    public class Ticket : ModelBase<Guid>
    {
        public string type { get; set; } //TODO : make an enum
        public string subject { get; set; }
        public string description { get; set; }
        public string priority { get; set; } //TODO :make an enum
        public string status { get; set; } //TODO: make an enum
        public int submitter_id { get; set; }
        public int assignee_id { get; set; }
        public int organization_id { get; set; }
        public bool has_incidents { get; set; }
        public DateTimeOffset due_at { get; set; }
        public string via { get; set; } //TODO: enum

    }
}
