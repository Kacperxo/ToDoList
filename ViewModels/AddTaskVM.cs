using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToDoList.Commands;
using ToDoList.Models;
using ToDoList.Services;


namespace ToDoList.ViewModels
{
    public class AddTaskVM : BaseViewModel, INotifyDataErrorInfo
    {
        #region Validation (INotifyDataErrorInfo)
        #region Validation variables
        private readonly Dictionary<string, List<string>> _errors = new();

        private bool _isTouched = false;
        public bool IsFormValid
        {
            get
            {
                // Sprawdź wszystkie warunki walidacji bez dodawania błędów do UI
                if (string.IsNullOrWhiteSpace(_Title)) return false;
                if (_Title.Length > 100) return false;
                if (Description.Length > 1000) return false;
                return true; 
            }
        }
        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        #endregion

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors.GetValueOrDefault(propertyName ?? "", new List<string>());
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_isTouched) return;
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }
            if (!_errors[propertyName].Contains(errorMessage))
            {
                _errors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        // Metoda do walidacji tytułu zadania
        private void ValidateTitle()
        {
            ClearErrors(nameof(Title));
            if (string.IsNullOrWhiteSpace(_Title))
            {
                AddError(nameof(Title), "Tytuł nie może być pusty.");
            }
            if (_Title.Length > 100)
            {
                AddError(nameof(Title), "Tytuł nie może mieć więcej niż 100 znaków.");
            }
        }

        // Metoda do walidacji opisu zadania
        private void ValidateDescription()
        {
            ClearErrors(nameof(Description));
            if (_Description.Length > 1000)
            {
                AddError(nameof(Description), "Opis nie może mieć więcej niż 1000 znaków.");
            }
        }
        #endregion

        #region view variables
        // ===== Tytuł =====
        private string _Title = "";
        public string Title
        {
            get => _Title;
            set
            {
                _isTouched = true;
                _Title = value;
                OnProp();
                OnProp(nameof(IsFormValid));
                ValidateTitle();
            }
        }

        // ===== Opis =====
        private string _Description = "";
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnProp();
                OnProp(nameof(IsFormValid));
                ValidateDescription();
            }
        }

        // ====== Data wykonania ======
        private DateTime _SelectedDueDate = DateTime.Today;
        public DateTime SelectedDueDate
        {
            get => _SelectedDueDate;
            set
            {
                _SelectedDueDate = value;
                OnProp();
            }
        }

        // ====== Czas wykonania ======
        private DateTime _SelectedDueTime = DateTime.Now;
        public DateTime SelectedDueTime
        {
            get => _SelectedDueTime;
            set
            {
                _SelectedDueTime = value;
                OnProp();
            }
        }
        #endregion

        #region Methods
        // ========== Add Task Button ==========
        public ICommand AddTaskButton { get; }
        private async Task AddTask()
        {
            TaskModel newTask = new TaskModel(_Title, _Description, SelectedDueDate.Date + SelectedDueTime.TimeOfDay);

            await _taskRepository.AddAsync(newTask);

            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Zadanie zostało dodane.",
                "Dodano zadanie",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);

            _ = _mainWindowVM.NavigateTo<TaskVM>();
        }

        // ========== Cancel Adding Task Button ==========
        public ICommand CancelAddingTaskButton { get; }
        private void CancelAddingTask()
        {
            _ = _mainWindowVM.NavigateTo<TaskVM>();
        }
        #endregion


        private readonly TaskRepository _taskRepository;
        private readonly MainWindowVM _mainWindowVM;

        public AddTaskVM(TaskRepository taskRepository, MainWindowVM mainWindowVM)
        {
            _taskRepository = taskRepository;
            _mainWindowVM = mainWindowVM;

            AddTaskButton = new AsyncRelayCommand(AddTask, () => IsFormValid);
            CancelAddingTaskButton = new RelayCommand(CancelAddingTask);
        }
    }
}
