using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class TaskVM : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;

        // ====== Lista zadań reagująca na zmiany ======
        private readonly ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();
        public ICollectionView TasksView { get; }

        // ====== Wybrana data ======
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set 
            { 
                _selectedDate = value; 
                OnPropertyChanged(); 
            }
        }

        // ====== Referencja do głównego okna ======
        public MainWindowVM? Root { get; set; }

        // ====== Zadanie aktualnie zaznaczone ======
        private TaskModel? _selectedTask;
        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                // Poinformuj MainWindowVM, że zaznaczenie się zmieniło
                Root?.OnTaskSelectionChanged(value);
            }
        }

        // ====== Konstruktor ======
        public TaskVM(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            TasksView = CollectionViewSource.GetDefaultView(_tasks);
            TasksView.SortDescriptions.Add(new SortDescription(nameof(TaskModel.IsCompleted), ListSortDirection.Ascending));
            TasksView.SortDescriptions.Add(new SortDescription(nameof(TaskModel.DueDate), ListSortDirection.Ascending));
        }

        #region Task List Interaction
        // ====== Ładowanie zadań dla wybranej daty ======
        public async Task LoadTasksForDate(DateTime date)
        {
            SelectedDate = date;

            foreach (var task in _tasks)
            {
                task.PropertyChanged -= OnTaskPropertyChanged;
            }
            _tasks.Clear();

            var tasksFromDb = await _taskRepository.GetByDateAsync(date);
            foreach (var task in tasksFromDb)
            {
                task.PropertyChanged += OnTaskPropertyChanged;
                _tasks.Add(task);
            }
            OnPropertyChanged(nameof(TasksView));
        }

        // ====== Obsługa zmiany właściwości IsCompleted ====== 
        private async void OnTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskModel.IsCompleted) && sender is TaskModel changedTask)
            {
                await _taskRepository.UpdateAsync(changedTask);
                TasksView.Refresh();
            }
        }
        #endregion
    }
}
