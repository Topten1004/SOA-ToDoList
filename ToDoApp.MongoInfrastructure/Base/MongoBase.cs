using System.Collections.Generic;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.MongoLogger.Base
{
    public abstract class MongoBase<T, TI> where TI : ISearchArgs
    {
        private IMongoDatabase _db;
        public string ConnectionString { get; set; }

        protected MongoBase()
        {
            var conventions = new ConventionPack { new IgnoreIfNullConvention(true) };
            ConventionRegistry.Register("IgnoreIfNull", conventions, x => true);
            try
            {
                //DateTimeSerializerKindUnspecified 2 kere register edilmeye çalışılırsa hataya sebep oluyor. BsonSerializer.RegisterSerializer() ile register etmeden önce 
                //BsonSerializer.LookupSerializer() methodu çağırıldığında da yine hata oluşuyor. bu sebeple reflection kullanıldı. 
                //oluşan hata: "There is already a serializer registered for type System.DateTime."

                //var serializersFieldInfo = (((FieldInfo[])(typeof(BsonSerializer).GetRuntimeFields())).First(x => x.Name == "__serializers"));
                //Dictionary<Type, IBsonSerializer> serializers = serializersFieldInfo.GetValue(null) as Dictionary<Type, IBsonSerializer>;

                //if (serializers != null && !serializers.Any(x => x.Value is DateTimeSerializerKindUnspecified))
                //    BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializerKindUnspecified());
            }
            catch
            {
                //ignored
            }
        }
        public IMongoDatabase GetDatabase()
        {
            if (_db != null)
                return _db;

            MongoUrl mongoUrl = MongoUrl.Create(ConnectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(mongoUrl);

            MongoClient client = new MongoClient(settings);
            IMongoDatabase db = client.GetDatabase(mongoUrl.DatabaseName);
            _db = db;
            return _db;
        }

        public IMongoCollection<T> GetCollection(string collectionName)
        {
            var db = GetDatabase();
            return db.GetCollection<T>(collectionName);
        }

        public bool Save(string collectionName, T item)
        {
            try
            {
                var collection = GetCollection(collectionName);
                collection.InsertOne(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(string collectionName, IEnumerable<T> items)
        {
            try
            {
                var collection = GetCollection(collectionName);
                collection.InsertMany(items);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public abstract IEnumerable<T> Search(string collectionName, TI searchArgs);
        public abstract T GetOne(string collectionName, TI searchArgs);
    }
}
