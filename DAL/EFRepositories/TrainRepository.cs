using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFRepositories
{
    internal class TrainRepository : ITrainRepository
    {
        private readonly ApplicationContext _context;

        public TrainRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(Train entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Train entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Train Get(int id)
        {
            return _context.Trains
                //.AsNoTracking()
                .FirstOrDefault(tr => tr.Id == id);
        }

        public IEnumerable<Train> GetAll()
        {
            return _context.Trains
                .AsNoTracking()
                .ToList();
        }

        public void Update(Train entity)
        {
            _context.Update(entity);
            _context.SaveChanges(); 
        }
    }
}
