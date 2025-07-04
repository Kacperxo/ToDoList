using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class ReminderRepository
    {
        private readonly AppDbContext _context;

        public ReminderRepository(AppDbContext context)
        {
            _context = context;
        }

        /* ----------  C  |  Create  ---------- */
        public async Task<ReminderModel> AddAsync(ReminderModel reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        /* ----------  R  |  Read  ------------ */
        public async Task<ReminderModel?> GetByIdAsync(int id)
        {
            return await _context.Reminders.FindAsync(id);
        }

        public async Task<IEnumerable<ReminderModel>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Reminders
                .Where(r => r.TaskId == taskId)
                .OrderBy(r => r.ReminderDateTime)
                .ToListAsync();
        }

        /* ----------  U  |  Update  ---------- */
        public async Task UpdateAsync(ReminderModel reminder)
        {
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
        }

        /* ----------  D  |  Delete  ---------- */
        public async Task DeleteAsync(ReminderModel reminder)
        {
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}