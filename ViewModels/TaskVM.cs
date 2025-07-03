using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class TaskVM : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();
        public ICollectionView TasksView { get; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnProp(); }
        }

        public MainWindowVM? Root { get; set; }

        private TaskModel? _selectedTask;
        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnProp();
                // Poinformuj MainWindowVM, że zaznaczenie się zmieniło
                Root?.OnTaskSelectionChanged(value);
            }
        }

        public TaskVM(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            TasksView = CollectionViewSource.GetDefaultView(_tasks);
            TasksView.SortDescriptions.Add(new SortDescription(nameof(TaskModel.IsCompleted), ListSortDirection.Ascending));
            TasksView.SortDescriptions.Add(new SortDescription(nameof(TaskModel.DueDate), ListSortDirection.Ascending));
        }

        // Publiczna metoda do ładowania/odświeżania zadań
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
            OnProp(nameof(TasksView));
        }

        private async void OnTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskModel.IsCompleted) && sender is TaskModel changedTask)
            {
                await _taskRepository.UpdateAsync(changedTask);
                TasksView.Refresh();
            }
        }
    }
}
