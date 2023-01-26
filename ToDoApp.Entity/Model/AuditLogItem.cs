using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoApp.Entity.Model
{
    public class AuditLogItem<TId, T>
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public TId EntityId { get; set; }
        public string EntityName { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string TakenBy { get; set; }
        public string ClientIp { get; set; }
        public string HostName { get; set; }
        public DateTime Timestamp { get; set; }
        public T Entity { get; set; }
        public object ReturnValue { get; set; }
        public double ExecutionMiliseconds { get; set; }
    }
}
