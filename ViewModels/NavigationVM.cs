using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList.Commands;

namespace ToDoList.ViewModels
{
    public class NavigationVM : BaseViewModel
    {
        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnProp();
                _ = _root.OnDateChanged(value);
            }
        }

        private readonly MainWindowVM _root;
        public ICommand AddButton { get; }
        public ICommand EditButton { get; }
        public ICommand DeleteButton { get; }

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
