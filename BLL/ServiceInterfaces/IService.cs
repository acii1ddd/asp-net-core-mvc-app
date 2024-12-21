namespace BLL.ServiceInterfaces
{
    public interface IService<T> where T : IDTO
    {
        void Add(T item);

        void Update(T item);

        void DeleteById(int id);

        T GetById(int id);

        IEnumerable<T> GetAll();
    }
}
