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

            services.AddSingleton<MainWindowVM>();

            IServiceProvider provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            var mainViewModel = provider.GetRequiredService<MainWindowVM>();

            MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
        }
    }
}
