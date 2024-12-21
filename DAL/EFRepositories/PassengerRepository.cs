using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFRepositories
{
    internal class PassengerRepository : IPassengerRepository
    {
        private readonly ApplicationContext _context = null!; // не будет null при использовании, а только при инициализации

        public PassengerRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException();
        }

        public void Add(Passenger entity)
        {
            _context.Passengers.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Passenger entity)
        {
            _context.Passengers.Remove(entity);
            _context.SaveChanges();
        }

        public Passenger Get(int id)
        {
            return _context.Passengers
                //.AsNoTracking()
                .FirstOrDefault(pas => pas.Id == id); // null by deafault
        }

        public IEnumerable<Passenger> GetAll()
        {
            return _context.Passengers.
                //AsNoTracking().
                ToList();
        }

        public void Update(Passenger entity)
        {
            _context.Passengers.Update(entity);
            _context.SaveChanges();
        }
    }
}
