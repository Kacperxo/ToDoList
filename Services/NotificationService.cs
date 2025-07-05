using System.Windows.Threading;

namespace ToDoList.Services
{
    public class NotificationService
    {
        private readonly TaskRepository _taskRepository;
        private readonly Notifier _notifier;
        private readonly DispatcherTimer _timer;

        public NotificationService(TaskRepository taskRepository, Notifier notifier)
        {
            _taskRepository = taskRepository;
            _notifier = notifier;

            _timer = new DispatcherTimer
            {
                // Sprawdzaj co 30 sekund
                Interval = TimeSpan.FromSeconds(30)
            };
            _timer.Tick += async (s, e) => await CheckForUpcomingTasks();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private async Task CheckForUpcomingTasks()
        {
            DateTime now = DateTime.Now;
            DateTime oneHourFromNow = now.AddHours(1);

            var upcomingTasks = await _taskRepository.GetUpcomingTasksAsync(now, oneHourFromNow);

            foreach (var task in upcomingTasks)
            {
                _notifier.ShowToast("Nadchodzące zadanie!", $"Zadanie '{task.Title}' jest do wykonania o {task.DueDate:HH:mm}.");
                task.IsNotificationSent = true;
                await _taskRepository.UpdateAsync(task);
            }
        }
    }
}