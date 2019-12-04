
namespace TicketCore.Dto
{
    public class SearchHit
    {
        public string DocId { get; set; }
        public string DocType { get; set; }
        public float Score { get; set; }
    }
}
