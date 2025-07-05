using System.Windows.Input;
using ToDoList.Commands;

namespace ToDoList.ViewModels
{
    public class NavigationVM : BaseViewModel
    {
        // ====== Ostatnio wybrana data ======
        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                // ====== Zdarzenie zmiany daty ======
                _selectedDate = value;
                OnPropertyChanged();
                _root.OnDateChanged(value);
            }
        }

        private readonly MainWindowVM _root;

        // ====== Komendy do przycisków ======
        public ICommand AddButton { get; }
        public ICommand EditButton { get; }
        public ICommand DeleteButton { get; }

        // ====== Konstruktor ======
        public NavigationVM(MainWindowVM root)
        {
            _root = root;

            AddButton = new AsyncRelayCommand(() => _root.NavigateTo<AddTaskVM>());

            EditButton = new RelayCommand(
                execute: () => _root.NavigateToEditView(),
                canExecute: () => _root.SelectedTask != null
);

            DeleteButton = new AsyncRelayCommand(
                execute: () => _root.DeleteSelectedTaskAsync(),
                canExecute: () => _root.SelectedTask != null);
        }
    }
}
