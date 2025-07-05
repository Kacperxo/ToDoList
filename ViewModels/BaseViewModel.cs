using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ToDoList.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // ====== Metoda do wywoływania zdarzenia PropertyChanged ======
        protected void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
