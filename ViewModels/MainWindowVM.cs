using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    /// <summary>
    /// Root-ViewModel aplikacji.  Zawiera nawigację i bieżącą stronę.
    /// </summary>
    public class MainWindowVM : BaseViewModel
    {
        // --- DI ------------------------------------------------------------
        private readonly IServiceProvider _provider;
        private readonly TaskRepository _taskRepository;

        // --- Bieżąca strona (ContentControl w MainWindow.xaml) -------------
        private object _currentPage = null!;
        public object CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnProp(); }
        }

        // --- Lewy panel z kalendarzem i przyciskami ------------------------
        public NavigationVM Navigation { get; }

        public TaskModel? SelectedTask { get; private set; }

        // --- Konstruktor ---------------------------------------------------
        public MainWindowVM(IServiceProvider provider)
        {
            _provider = provider;

            _taskRepository = _provider.GetRequiredService<TaskRepository>();

            // 1. twórz panel nawigacji (dostaje referencję do "roota")
            Navigation = new NavigationVM(this);

            // 2. ustaw ekran startowy: lista zadań
            _ = NavigateTo<TaskVM>();
        }

        public void OnTaskSelectionChanged(TaskModel? task)
        {
            SelectedTask = task;
            CommandManager.InvalidateRequerySuggested();
        }

        public void NavigateToEditView()
        {
            if (SelectedTask == null) return;

            var editTaskVM = new EditTaskVM(SelectedTask, _taskRepository, this);

            Navigate(editTaskVM);
        }

        public async Task DeleteSelectedTaskAsync()
        {
            if (SelectedTask == null) return;
            MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz usunąć to zadanie?",
                "Potwierdzenie usunięcia",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) 
                return;
            else
                await _taskRepository.DeleteAsync(SelectedTask);

            // Odśwież listę zadań po usunięciu
            if (CurrentPage is TaskVM taskVM)
            {
                await taskVM.LoadTasksForDate(Navigation.SelectedDate);
            }
        }

        // --- Metody nawigacji ---------------------------------------------
        /// <summary>
        /// Przejdź na stronę typu <typeparamref name="T"/> pobieraną z DI.
        /// </summary>
        public async Task NavigateTo<T>() where T : BaseViewModel
        {
            CurrentPage = _provider.GetRequiredService<T>();

            if(CurrentPage is TaskVM taskVM)
            {
                taskVM.Root = this;
                await taskVM.LoadTasksForDate(Navigation.SelectedDate);
            }
        }

        public async Task OnDateChanged(DateTime newDate)
        {
            if (CurrentPage is TaskVM taskVM)
            {
                await taskVM.LoadTasksForDate(newDate);
            }
        }

        /// <summary>
        /// Przejdź na już utworzony ViewModel (np. gdy potrzebujesz konstruktora z parametrem).
        /// </summary>
        public void Navigate(object vm) => CurrentPage = vm;
    }
}
