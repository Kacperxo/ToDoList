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
        // ====== Słownik do przechowywania błędów walidacji ======
        private readonly Dictionary<string, List<string>> _errors = new();

        // ====== Flaga, czy formularz został dotknięty ======
        // ======
        private bool _isTouched = false;
        public bool IsFormValid
        {
            get
            {
                // ====== Sprawdzenie, czy formularz jest poprawny przed dodaniem bledow do UI ======
                if (string.IsNullOrWhiteSpace(_Title)) return false;
                if (_Title.Length > 100) return false;
                if (Description.Length > 1000) return false;
                return true; 
            }
        }
        // ====== Sprawdzenie, czy formularz ma błędy ======
        public bool HasErrors => _errors.Any();

        // ====== Zdarzenie do powiadamiania o zmianach błędów ======
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        #endregion

        // ====== Metoda do wywoływania zdarzenia ErrorsChanged ======
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        // ====== Metoda do sprawdzania błędów walidacji ======
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors.GetValueOrDefault(propertyName ?? "", new List<string>());
        }

        // ====== Metoda do dodawania błędów walidacji ======
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

        // ====== Metoda do usuwania błędów walidacji ======
        private void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        // ====== Metoda do walidacji tytułu zadania ======
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

        // ====== Metoda do walidacji opisu zadania ======
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFormValid));
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFormValid));
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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

        // ====== Konstruktor ======
        public AddTaskVM(TaskRepository taskRepository, MainWindowVM mainWindowVM)
        {
            _taskRepository = taskRepository;
            _mainWindowVM = mainWindowVM;

            AddTaskButton = new AsyncRelayCommand(AddTask, () => IsFormValid);
            CancelAddingTaskButton = new RelayCommand(CancelAddingTask);
        }
    }
}
