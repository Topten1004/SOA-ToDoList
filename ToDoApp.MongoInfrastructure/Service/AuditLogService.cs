using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoApp.Core.Service.Log;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;
using ToDoApp.MongoLogger.Base;

namespace ToDoApp.MongoLogger.Service
{
    public class AuditLogService : MongoBase<AuditLogItem<int, object>, AuditLogSearchArgs<int>>, IAuditLogService
    {
        public AuditLogService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override IEnumerable<AuditLogItem<int, object>> Search(string collectionName, AuditLogSearchArgs<int> searchArgs)
        {
            IEnumerable<AuditLogItem<int, object>> result;
            try
            {
                var query = PrepareMongoQuery(ref searchArgs);
                var collection = GetCollection(collectionName);
                long count;
                if (query != null)
                {
                    count = collection.Count(query);
                    result = collection.Find(query).SortByDescending(x => x.Timestamp).Skip((int)searchArgs.Skip).Limit((int)searchArgs.Take).ToList();
                }
                else
                {
                    count = collection.Count(null);
                    result = collection.Find(null).SortByDescending(x => x.Timestamp).Skip((int)searchArgs.Skip).Limit((int)searchArgs.Take).ToList();
                }
                searchArgs.Count = count;
            }
            catch
            {
                return null;
            }

            return result;
        }

        public override AuditLogItem<int, object> GetOne(string collectionName, AuditLogSearchArgs<int> searchArgs)
        {
            var query = PrepareMongoQuery(ref searchArgs);
            var collection = GetCollection(collectionName);
            if (query == null)
                return null;

            return collection.Find(query).Limit(1).FirstOrDefault();
        }

        private static FilterDefinition<AuditLogItem<int, object>> PrepareMongoQuery(ref AuditLogSearchArgs<int> searchArgs)
        {
            FilterDefinitionBuilder<AuditLogItem<int, object>> builder = new FilterDefinitionBuilder<AuditLogItem<int, object>>();

            if (searchArgs == null)
            {
                searchArgs = new AuditLogSearchArgs<int>
                {
                    Take = int.MaxValue,
                    Skip = 0
                };
                return builder.Empty;
            }

            IList<FilterDefinition<AuditLogItem<int, object>>> ands = new List<FilterDefinition<AuditLogItem<int, object>>>();

            if (searchArgs.Id != null)
            {
                ObjectId id = ObjectId.Parse(searchArgs.Id);
                ands.Add(builder.Eq("_id", id));
            }

            //if (!string.IsNullOrEmpty(searchArgs.Symbol))
            //    ands.Add(builder.Eq("Symbol", searchArgs.Symbol));

            //if (searchArgs.Id.HasValue)
            //    ands.Add(builder.Eq("Id", searchArgs.Id));

            return builder.And(ands);

            //if (searchArgs.From.HasValue)
            //    ands.Add(Query.GTE("Timestamp", BsonValue.Create(searchArgs.From)));

            //if (searchArgs.To.HasValue)
            //    ands.Add(Query.LTE("Timestamp", BsonValue.Create(searchArgs.To)));
        }
    }
}
