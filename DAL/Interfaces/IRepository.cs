namespace DAL.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);

        void Update(T entity);

        T Get(int id);

        void Delete(T entity);

        IEnumerable<T> GetAll();
    }
}
