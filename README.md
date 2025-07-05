# ToDoList

Prosta aplikacja WPF do zarządzania listą zadań, zbudowana z wykorzystaniem wzorca MVVM.

## Funkcjonalności

*   **Dodawanie, edytowanie i usuwanie zadań:** Pełna obsługa operacji CRUD na zadaniach.
*   **Widok dzienny:** Przeglądaj zadania na wybrany dzień za pomocą wbudowanego kalendarza.
*   **Oznaczanie zadań jako ukończone:** Śledź postępy, zaznaczając wykonane zadania.
*   **Walidacja formularzy:** Wbudowana walidacja danych wejściowych, aby zapobiec błędom.
*   **Powiadomienia:** Otrzymuj powiadomienia na pulpicie o nadchodzących zadaniach.
*   **Szczegóły zadania:** Wyświetlaj opis wybranego zadania w dedykowanym panelu.

## Technologie

*   **.NET 8**
*   **WPF** (Windows Presentation Foundation)
*   **Entity Framework Core** (ORM) z bazą danych **SQLite**
*   **Wzorzec MVVM** (Model-View-ViewModel)
*   **Wstrzykiwanie zależności** (Dependency Injection)
*   **Extended.Wpf.Toolkit** (dla kontrolki `TimePicker`)
*   **Microsoft.Toolkit.Uwp.Notifications** (dla powiadomień toastowych)

## Struktura projektu

*   `Commands/`: Implementacje `ICommand` (`RelayCommand`, `AsyncRelayCommand`) do obsługi akcji użytkownika.
*   `Data/`: Konfiguracja `DbContext` dla Entity Framework Core.
*   `Migrations/`: Migracje bazy danych generowane przez EF Core.
*   `Models/`: Modele danych (np. `TaskModel`).
*   `Notifier/`: Klasa do obsługi powiadomień systemowych.
*   `Services/`: Logika biznesowa, w tym repozytorium (`TaskRepository`) i serwis powiadomień (`NotificationService`).
*   `ViewModels/`: ViewModele, które zawierają logikę i stan dla poszczególnych widoków.
*   `Views/`: Kontrolki użytkownika (UserControl) definiujące interfejs aplikacji w XAML.

## Uruchomienie gotowej aplikacji

1.  Przejdź na stronę `https://github.com/Kacperxo/ToDoList/releases/tag/1.0.0`.
2.  W sekcji `Assets` kliknij `ToDoList.zip`, aby pobrać plik.
3.  Wypakuj pobrane archiwum.
4.  W folderze `ToDoList` uruchom plik `ToDoList.exe`.

## Uruchomienie ze źródeł

1.  Sklonuj repozytorium.
2.  Otwórz plik `ToDoList.sln` w programie Visual Studio.
3.  Zbuduj projekt (spowoduje to przywrócenie pakietów NuGet).
4.  Uruchom aplikację. Baza danych `database.db` zostanie automatycznie utworzona w głównym folderze projektu przy pierwszym uruchomieniu.
