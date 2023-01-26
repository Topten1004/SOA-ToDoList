using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Common.Attributes;
using ToDoApp.Common.Service;
using ToDoApp.Entity.Model;
using Task = ToDoApp.Entity.Model.Task;

namespace ToDoApp.Entity.Context
{
    public class ToDoAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Task> Tasks { get; set; }

        #region Encryption

        private readonly EncryptionService _encryptionService;

        public ToDoAppContext()
        {
            _encryptionService = new EncryptionService("HdfA03Zr04rAod21L+", "cRea27JeT", "*!&@^#%$");
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ObjectMaterialized;
        }

        public override int SaveChanges()
        {
            var contextAdapter = ((IObjectContextAdapter)this);

            contextAdapter.ObjectContext.DetectChanges();

            var pendingEntities = contextAdapter.ObjectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                .Where(en => !en.IsRelationship).ToList();

            foreach (var entry in pendingEntities)
                EncryptEntity(entry.Entity);

            int result = base.SaveChanges();

            foreach (var entry in pendingEntities)
                DecryptEntity(entry.Entity);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var contextAdapter = ((IObjectContextAdapter)this);

            contextAdapter.ObjectContext.DetectChanges();

            var pendingEntities = contextAdapter.ObjectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                .Where(en => !en.IsRelationship).ToList();

            foreach (var entry in pendingEntities)
                EncryptEntity(entry.Entity);

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entry in pendingEntities)
                DecryptEntity(entry.Entity);

            return result;
        }

        public void ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            DecryptEntity(e.Entity);
        }

        private void EncryptEntity(object entity)
        {
            var encryptedProperties = entity.GetType().GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(Encrypt), true).Any(a => p.PropertyType == typeof(string)));
            foreach (var property in encryptedProperties)
            {
                string value = property.GetValue(entity) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    string encryptedValue = _encryptionService.Encrypt(value);
                    property.SetValue(entity, encryptedValue);
                }
            }
        }

        private void DecryptEntity(object entity)
        {
            var encryptedProperties = entity.GetType().GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(Encrypt), true).Any(a => p.PropertyType == typeof(string)));

            foreach (var property in encryptedProperties)
            {
                string encryptedValue = property.GetValue(entity) as string;
                if (!String.IsNullOrEmpty(encryptedValue))
                {
                    string value = _encryptionService.Decrypt(encryptedValue);
                    Entry(entity).Property(property.Name).OriginalValue = value;
                    Entry(entity).Property(property.Name).IsModified = false;
                }
            }
        }



        #endregion Encryption
    }
}
