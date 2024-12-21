using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFRepositories
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(Ticket entity)
        {
            _context.Tickets.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Ticket entity)
        {
            _context.Tickets.Remove(entity);
            _context.SaveChanges();
        }

        public Ticket Get(int id)
        {
            return _context.Tickets
                //.AsNoTracking()
                .FirstOrDefault(t => t.Id == id); // ef не будет следить за изменениями в объектах (они будут находиться в только для чтения режиме)
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _context.Tickets
                .AsNoTracking()
                .ToList();
        }

        public void Update(Ticket entity)
        {
            _context.Tickets.Update(entity);
            _context.SaveChanges();
        }
    }
}
