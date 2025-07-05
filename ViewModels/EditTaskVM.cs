using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using ToDoList.Commands;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class EditTaskVM : BaseViewModel, INotifyDataErrorInfo
    {
        #region Validation (INotifyDataErrorInfo)
        #region Validation variables
        // ====== Słownik do przechowywania błędów walidacji ======
        private readonly Dictionary<string, List<string>> _errors = new();

        // ====== Flaga, czy formularz został dotknięty ======
        private bool _isTouched = false;
        public bool IsFormValid
        {
            get
            {
                // ====== Sprawdzenie, czy formularz jest poprawny przed dodaniem błędów do UI ======
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
        // ====== Tytuł ======
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

        // ====== Opis ======
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
        // ====== Save Task Button ======
        public ICommand SaveTaskButton { get; }
        private async Task SaveTask()
        {
            _originalTask.Title = this.Title;
            _originalTask.Description = this.Description;
            _originalTask.DueDate = this.SelectedDueDate.Date + this.SelectedDueTime.TimeOfDay;

            await _taskRepository.UpdateAsync(_originalTask);

            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Zadanie zostało zapisane.",
                "Zapisano",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);

            await _mainWindowVM.NavigateTo<TaskVM>();
        }

        // ====== Cancel Saving Task Button ======
        public ICommand CancelSavingTaskButton { get; }
        private async void CancelSavingTask()
        {
            await _mainWindowVM.NavigateTo<TaskVM>();
        }
        #endregion


        private readonly TaskRepository _taskRepository;
        private readonly MainWindowVM _mainWindowVM;
        private TaskModel _originalTask;

        // ====== Konstruktor ======
        public EditTaskVM(TaskModel taskToEdit, TaskRepository taskRepository, MainWindowVM mainWindowVM)
        {
            _originalTask = taskToEdit;
            _taskRepository = taskRepository;
            _mainWindowVM = mainWindowVM;

            _Title = _originalTask.Title;
            _Description = _originalTask.Description;
            _SelectedDueDate = _originalTask.DueDate.Date;
            _SelectedDueTime = DateTime.Today + _originalTask.DueDate.TimeOfDay;

            SaveTaskButton = new AsyncRelayCommand(SaveTask, () => IsFormValid);
            CancelSavingTaskButton = new RelayCommand(CancelSavingTask);
        }
    }
}
