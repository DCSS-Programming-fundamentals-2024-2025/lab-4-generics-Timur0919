namespace generics.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        void Add(TKey id, TEntity entity);
        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();
        void Remove(TKey id);
    }

    public interface IReadOnlyRepository<out TEntity, in TKey>
    {
        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();
    }

    public interface IWriteRepository<in TEntity, in TKey>
    {
        void Add(TEntity entity);
        void Remove(TKey id);
    }
}
