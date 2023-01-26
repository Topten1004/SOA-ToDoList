using System;

namespace ToDoApp.Contract
{
    [Serializable]
    public class AuditLogItemContract
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string TakenBy { get; set; }
        public string ClientIp { get; set; }
        public string HostName { get; set; }
        public DateTime Timestamp { get; set; }
        public object Entity { get; set; }
        public object ReturnValue { get; set; }
        public double ExecutionMiliseconds { get; set; }
    }
}
