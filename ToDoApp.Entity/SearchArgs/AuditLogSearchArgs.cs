using System;

namespace ToDoApp.Entity.SearchArgs
{
    public class AuditLogSearchArgs<TId> : ISearchArgs
    {
        public string Id { get; set; }
        public TId EntityId { get; set; }
        public string Action { get; set; }
        public string LocationCode { get; set; }
        public string TrackerCode { get; set; }
        public int? OrderId { get; set; }
        public string StatusExplanation { get; set; }
        public string TakenBy { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public uint Skip { get; set; }
        public uint Take { get; set; }
        public long Count { get; set; }
    }
}
