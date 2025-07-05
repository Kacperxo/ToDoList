using ToDoList.Data;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services
{
    public class TaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        /* ----------  C  |  Create  ---------- */
        public async Task<TaskModel> AddAsync(TaskModel task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        /* ----------  R  |  Read  ------------ */
        public async Task<IEnumerable<TaskModel>> GetByDateAsync(DateTime date)
        {
            return await _context.Tasks
                .Where(t => t.DueDate.Date == date.Date)
                .ToListAsync();
        }
        public async Task<List<TaskModel>> GetForDateAsync(DateTime day)
        {
            DateTime start = day.Date;
            DateTime end = start.AddDays(1);

            return await _context.Tasks
                             .Where(t => t.DueDate >= start && t.DueDate < end)
                             .OrderBy(t => t.IsCompleted)
                             .ThenBy(t => t.DueDate)
                             .ToListAsync();
        }

        public async Task<TaskModel?> GetByIdAsync(int id) =>
            await _context.Tasks.FindAsync(id);

        public async Task<IEnumerable<TaskModel>> GetUpcomingTasksAsync(DateTime from, DateTime to)
        {
            return await _context.Tasks
                .Where(t => t.DueDate > from && t.DueDate <= to && !t.IsCompleted && !t.IsNotificationSent)
                .ToListAsync();
        }

        /* ----------  U  |  Update  ---------- */
        public async Task UpdateAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        /* ----------  D  |  Delete  ---------- */
        public async Task DeleteAsync(TaskModel task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
