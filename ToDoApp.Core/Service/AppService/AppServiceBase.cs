using System;

namespace ToDoApp.Core.Service.AppService
{
    public abstract class AppServiceBase<T> where T : class
    {
        protected abstract void SetUpdateFields(T dbEntity, ref T entity);

        protected T SetEntityFields(T entity, T from, params string[] properties)
        {
            var type = entity.GetType();
            foreach (string field in properties)
            {
                if (type.GetProperty(field) == null)
                    throw new ArgumentException();
                var pInfo = type.GetProperty(field);
                if (pInfo == null)
                    continue;

                pInfo.SetValue(entity, pInfo.GetValue(from, null), null);
            }
            return entity;
        }
    }
}
