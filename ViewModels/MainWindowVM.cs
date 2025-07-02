using System;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoList.ViewModels
{
    /// <summary>
    /// Root-ViewModel aplikacji.  Zawiera nawigację i bieżącą stronę.
    /// </summary>
    public class MainWindowVM : BaseViewModel
    {
        // --- DI ------------------------------------------------------------
        private readonly IServiceProvider _provider;

        // --- Bieżąca strona (ContentControl w MainWindow.xaml) -------------
        private object _currentPage = null!;
        public object CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnProp(); }
        }

        // --- Lewy panel z kalendarzem i przyciskami ------------------------
        public NavigationVM Navigation { get; }

        // --- Konstruktor ---------------------------------------------------
        public MainWindowVM(IServiceProvider provider)
        {
            _provider = provider;

            // 1. twórz panel nawigacji (dostaje referencję do "roota")
            Navigation = new NavigationVM(this);

            // 2. ustaw ekran startowy: lista zadań
            CurrentPage = _provider.GetRequiredService<TaskVM>();
        }

        // --- Metody nawigacji ---------------------------------------------
        /// <summary>
        /// Przejdź na stronę typu <typeparamref name="T"/> pobieraną z DI.
        /// </summary>
        public void NavigateTo<T>() where T : class =>
            CurrentPage = _provider.GetRequiredService<T>();

        /// <summary>
        /// Przejdź na już utworzony ViewModel (np. gdy potrzebujesz konstruktora z parametrem).
        /// </summary>
        public void Navigate(object vm) => CurrentPage = vm;
    }
}
