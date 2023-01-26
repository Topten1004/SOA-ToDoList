namespace ToDoApp.Core.Persistence
{
    public interface IDatabaseContextFactory<out T>
    {
        T MasterDbContext();
    }
}
