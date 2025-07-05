using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        // ====== Wstrzykiwanie zależności ======
        private readonly IServiceProvider _provider;

        // ====== CRUD do TaskModel ======
        private readonly TaskRepository _taskRepository;

        // ====== Panel nawigacji ======
        public NavigationVM Navigation { get; }

        // ====== Zaznaczone zadanie ======
        public TaskModel? SelectedTask { get; private set; }

        // ====== Obiekt przechowujący aktualną stronę ======
        private object _currentPage = null!;
        public object CurrentPage
        {
            get => _currentPage;
            set 
            {
                // ====== Zdarzenie zmiany strony ======
                _currentPage = value; 
                OnPropertyChanged(); 
            }
        }

        // ====== Konstruktor głównego okna ======
        public MainWindowVM(IServiceProvider provider)
        {
            _provider = provider;

            _taskRepository = _provider.GetRequiredService<TaskRepository>();

            // 1. twórz panel nawigacji (dostaje referencję do "roota")
            Navigation = new NavigationVM(this);

            // 2. ustaw ekran startowy: lista zadań
            NavigateTo<TaskVM>();
        }

        #region PropertyChanged Events
        // ====== Zdarzenie zmiany zaznaczenia zadania ======
        public void OnTaskSelectionChanged(TaskModel? task)
        {
            SelectedTask = task;
            CommandManager.InvalidateRequerySuggested();
        }

        // ====== Zdarzenie zmiany daty w kalendarzu ======
        public async Task OnDateChanged(DateTime newDate)
        {
            if (CurrentPage is TaskVM taskVM)
            {
                await taskVM.LoadTasksForDate(newDate);
            }
        }
        #endregion


        #region NavigationVM Actions
        // ====== Nawigacja do widoku edytowania zadania ======
        public void NavigateToEditView()
        {
            if (SelectedTask == null) return;

            var editTaskVM = new EditTaskVM(SelectedTask, _taskRepository, this);

            Navigate(editTaskVM);
        }

        // ====== Usuwanie zaznaczonego zadania ======
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
        #endregion

        #region Navigation Methods
        /// <summary>
        /// Metoda służy do nawigacji, czyli zmiany aktualnie wyświetlanego widoku (strony).
        /// Pobiera z kontenera wstrzykiwania zależności (DI) nową instancję ViewModelu określonego przez typ 'T' i ustawia ją jako bieżącą stronę (CurrentPage).
        /// Parametr 'T' to typ ViewModelu, do którego ma nastąpić przejście (np. TaskVM, AddTaskVM).
        /// Jeśli nową stroną jest widok listy zadań (TaskVM), metoda dodatkowo ładuje zadania dla aktualnie wybranej daty.
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


        // ====== Nawigacja do innego widoku ======
        public void Navigate(object vm) => CurrentPage = vm;
        #endregion
    }
}
