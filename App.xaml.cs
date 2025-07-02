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

            IServiceProvider provider = services.BuildServiceProvider();

            MainWindow = new MainWindow
            {
                DataContext = new MainWindowVM(provider)
            };
            MainWindow.Show();
        }
    }
}
