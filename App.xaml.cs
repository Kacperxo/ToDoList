using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ToDoList.Data;
using ToDoList.Services;
using ToDoList.ViewModels;

namespace ToDoList
{
    public partial class App : Application
    {
        private NotificationService? _notificationService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();   

            services.AddTransient<TaskRepository>(); 
            services.AddTransient<TaskVM>();
            services.AddTransient<AddTaskVM>();
            services.AddTransient<EditTaskVM>();
            services.AddTransient<NavigationVM>();

            services.AddSingleton<Notifier>();
            services.AddSingleton<NotificationService>();

            services.AddSingleton<MainWindowVM>();

            IServiceProvider provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            var mainViewModel = provider.GetRequiredService<MainWindowVM>();

            _notificationService = provider.GetRequiredService<NotificationService>();
            _notificationService.Start();

            MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
        }
    }
}
