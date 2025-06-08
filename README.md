Projekt zaliczeniowy z ASP.NET Core - kompleksowa aplikacja webowa do zarządzania warsztatem samochodowym.
Funkcjonalności systemu
🔐 System ról i uprawnień
Aplikacja obsługuje trzy typy użytkowników:

Administrator - pełny dostęp do wszystkich funkcji

Recepcjonista - obsługa klientów i przyjmowanie zleceń

Mechanik - wykonywanie napraw i aktualizacja statusów


👤 Zarządzanie klientami

Rejestracja nowych klientów
Edycja danych kontaktowych
Wyszukiwanie w bazie klientów
Podgląd historii pojazdów każdego klienta

🚘 Obsługa pojazdów

Dodawanie pojazdów z danymi (VIN, numer rejestracyjny)
Upload i przechowywanie zdjęć pojazdów
Powiązanie pojazdu z właścicielem

🧾 System zleceń serwisowych

Tworzenie nowych zleceń napraw
Zarządzanie statusami (przyjęte, w trakcie, zakończone)
Przypisywanie konkretnych mechaników do zleceń
Śledzenie postępu prac

🔧 Czynności i części zamienne

Katalog dostępnych usług serwisowych z cenami robocizny
Baza części zamiennych z kosztami
Wybór i przypisywanie części do konkretnych napraw
Automatyczne kalkulowanie kosztów

💬 System komentarzy

Wewnętrzne komentarze do każdego zlecenia
Historia wszystkich działań i notatek
Komunikacja między mechanikmi a recepcją

📈 Raporty i analizy

Raporty kosztów napraw dla klientów
Zestawienia miesięczne i roczne
Analizy dla konkretnych pojazdów
Eksport wszystkich raportów do PDF

Zaawansowane funkcje techniczne
⚡ Optymalizacja wydajności

Indeksy bazodanowe - optymalizacja często wykonywanych zapytań
SQL Profiler - monitorowanie i analiza zapytań do bazy danych
Testy wydajności NBomber - symulacja obciążenia z 50 równoległymi użytkownikami

🤖 Automatyzacja procesów

GitHub Actions CI/CD - automatyczne budowanie, testowanie
BackgroundService - codzienne generowanie i wysyłanie raportów mailem
NLog - zaawansowane logowanie błędów i zdarzeń systemowych

Klienci → Pojazdy → Zlecenia → Czynności → Części + Komentarze

Każde zlecenie może zawierać wiele czynności, każda czynność może wymagać różnych części, a cały proces jest dokumentowany przez komentarze i logi systemowe.

System logowania i rejestracji
🔐 Logowanie
Formularz logowania

Pole Email - identyfikacja użytkownika
Pole Hasło - autoryzacja (ukryte znaki)
Przycisk "Zaloguj" - wysłanie danych
Link do rejestracji - dla nowych użytkowników

Funkcje

Walidacja pól w czasie rzeczywistym
Wyświetlanie błędów walidacji pod każdym polem
Przekierowanie do panelu po udanym logowaniu

📝 Rejestracja
Formularz rejestracji

Email - adres e-mail nowego użytkownika
Hasło - zabezpieczone pole tekstowe
Potwierdź hasło - weryfikacja poprawności hasła
Przycisk "Zarejestruj" - utworzenie konta

Zabezpieczenia

Walidacja email (format, unikalność)
Walidacja hasła (długość, złożoność)
Potwierdzenie hasła (musi być identyczne)
Wyświetlanie wszystkich błędów walidacji

Role i uprawnienia w systemie
👨‍💼 Administrator
Pełny dostęp do wszystkich funkcji systemu:
Zarządzanie klientami:

Dodawanie nowych klientów
Edytowanie danych klientów
Przeglądanie listy wszystkich klientów

Zarządzanie użytkownikami:

Zarządzanie rolami innych użytkowników

Zarządzanie pojazdami:

Pełne zarządzanie pojazdami w systemie

Zarządzanie czynnościami serwisowymi:

Zarządzanie dostępnymi czynnościami
Przypisywanie czynności do zleceń

Zarządzanie zleceniami:

Tworzenie nowych zleceń
Zmiana statusu zleceń
Przeglądanie wszystkich zleceń

Pozostałe funkcje:

Przeglądanie komentarzy
Zarządzanie częściami zamiennymi
Generowanie raportów
Raporty napraw


👩‍💻 Recepcjonista
Obsługa klientów i podstawowe zarządzanie zleceniami:
Zarządzanie klientami:

Dodawanie nowych klientów
Edytowanie danych klientów
Przeglądanie listy klientów

Zarządzanie pojazdami:

Dodawanie pojazdów do klientów

Zarządzanie zleceniami:

Tworzenie nowych zleceń dla pojazdów

Raporty:

Generowanie raportów
Przeglądanie raportów napraw


🔧 Mechanik
Wykonywanie prac serwisowych:
Zarządzanie pojazdami:

Przeglądanie i zarządzanie pojazdami

Praca z zleceniami:

Dodawanie czynności serwisowych do zleceń
Zmiana statusu zleceń (np. "w trakcie", "zakończone")


Wspólne dla wszystkich ról:

Wylogowanie się z systemu
Dostęp do swojego profilu użytkownika
