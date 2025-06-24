# System Zarządzania Serwisem Samochodowym

Witaj w repozytorium **Systemu Zarządzania Serwisem Samochodowym**! 

Jest to aplikacja webowa, stworzona na projekt zaliczeniowy na przedmiot .NET (IV Semestr). Aplikacja ma na celu cyfryzację i usprawnienie procesów w nowoczesnym warsztacie samochodowym. 

##  Główne Założenia

Aplikacja nie jest jedynie prostym systemem CRUD. To rozwiązanie, które symuluje realne środowisko pracy serwisu. Wśród głównych funkcji znajdziesz:

-   **Zarządzanie Użytkownikami i Rolami:** System rozróżnia uprawnienia dla **Administratora**, **Recepcjonisty** i **Mechanika**, zapewniając bezpieczeństwo i kontrolę dostępu.
-   **Kompleksowa Obsługa Zleceń:** Od przyjęcia pojazdu, przez diagnozę, przypisanie zadań i części, aż po finalizację i raportowanie.
-   **Automatyzacja i Komunikacja:** System automatycznie wysyła powiadomienia e-mail i generuje raporty w formacie PDF, minimalizując papierkową robotę.
-   **Nowoczesny Stos Technologiczny:** Wykorzystanie najnowszych technologii z .NET gwarantuje wydajność, bezpieczeństwo i łatwość w dalszym rozwoju.

##  Kluczowe Funkcjonalności

-   **System Tożsamości:** Pełna obsługa rejestracji, logowania i autoryzacji w oparciu o **ASP.NET Core Identity**. Role (Admin, Recepcjonista, Mechanik) są automatycznie tworzone przy pierwszym uruchomieniu aplikacji.
-   **Zarządzanie Klientami (CRM):** Baza klientów serwisu z historią ich pojazdów.
-   **Zarządzanie Pojazdami:** Ewidencja pojazdów z możliwością dodawania **zdjęć**, co ułatwia identyfikację i dokumentację.
-   **Moduł Zleceń Serwisowych:**
    -   Tworzenie i śledzenie statusu zleceń (np. "Nowe", "W trakcie", "Zakończone").
    -   Przypisywanie mechaników do konkretnych zleceń.
    -   Dynamiczne dodawanie **zadań do wykonania** (np. "Wymiana oleju") oraz **części zamiennych** do zlecenia.
-   **API RESTful:**  API do zarządzania zasobami (Klienci, Pojazdy, Zamówienia), udokumentowane przy użyciu **Swaggera**.
-   **Generowanie PDF:** Usługa generująca raporty (np. podsumowania zleceń) w formacie PDF przy użyciu biblioteki **QuestPDF**.
-   **Powiadomienia E-mail:** Wbudowany `EmailService` do wysyłania powiadomień.
-   **Logowanie Błędów:** Zaawansowane logowanie zdarzeń i błędów w aplikacji za pomocą **NLog**.
-   **Konteneryzacja:** Pełne wsparcie dla **Dockera**, co umożliwia łatwe wdrożenie i skalowanie.

##  Stos Technologiczny

| Kategoria                | Technologia                                                                                             |
| ------------------------ | ------------------------------------------------------------------------------------------------------- |
| **Backend**              | .NET 8, ASP.NET Core (Web API, MVC, Razor Pages)                                                        |
| **Baza Danych**          | Microsoft SQL Server                                                                                    |
| **ORM**                  | Entity Framework Core 8                                                                                 |
| **Uwierzytelnianie**     | ASP.NET Core Identity                                                                                   |
| **API**                  | REST, Swagger (Swashbuckle)                                                                             |
| **Generowanie PDF**      | QuestPDF                                                                                                |
| **Logowanie**            | NLog                                                                                                    |
| **Konteneryzacja**       | Docker                                                                                                  |


##  Architektura i Wzorce Projektowe


-   **Warstwa Danych:** `ApplicationDbContext` (Entity Framework Core) zarządza komunikacją z bazą danych. Modele encji definiują strukturę danych, a relacje między nimi są precyzyjnie określone w `OnModelCreating`.
-   **Warstwa Logiki Biznesowej:** Serwisy (np. `EmailService`, `OrderReportService`) hermetyzują logikę biznesową, promując reużywalność kodu.
-   **Warstwa Prezentacji i API:**
    -   **Kontrolery API** (`[ApiController]`) udostępniają zasoby poprzez RESTful HTTP.
    -   **Kontrolery MVC i Strony Razor** odpowiadają za renderowanie interfejsu użytkownika.
-   **Wstrzykiwanie Zależności (DI):** Intensywnie wykorzystywane do zarządzania cyklem życia usług i zmniejszania powiązań między komponentami.
-   **DTOs i Mappery:** Użycie obiektów DTO (Data Transfer Objects) i mapperów (np. `VehicleMapper`) do oddzielenia modeli domeny od modeli widoku/API, co zwiększa bezpieczeństwo i elastyczność.

##  Dodatkowe narzędzia i technologie


###  1. Optymalizacja Wydajności Bazy Danych za Pomocą Indeksów
Sprawdziłem, jak działają kluczowe zapytania bez indeksów i po dodaniu indeksów nieklastrowanych. Po optymalizacji widać było poprawę wydajności – zapytania wykonywały się szybciej, co potwierdziła analiza planów zapytań.

![image](https://github.com/user-attachments/assets/04118703-c512-4c6e-9e99-61cd7ba9286f)
![image](https://github.com/user-attachments/assets/e5112bd7-b004-467c-99d2-a39fec1698dc)


###  2. Monitoring i Analiza Zapytań SQL
W celu pełnej kontroli nad interakcją aplikacji z bazą danych, wykorzystano **SQL Server Profiler** oraz mechanizmy logowania zapytań w **Entity Framework Core**. Pozwoliło to na "podsłuchanie" i analizę surowych zapytań SQL generowanych przez kod dla konkretnych endpointów API. Dzięki temu mamy pewność, że ORM tłumaczy zapytania w sposób optymalny.

![image](https://github.com/user-attachments/assets/f1b51ba1-90cd-4494-81e0-b16d99688592)
![image](https://github.com/user-attachments/assets/04a94966-e70d-45be-ae6e-c7d9c4a5b4d8)


###  3. Automatyzacja Procesów z GitHub Actions (CI/CD)
Projekt wykorzystuje **GitHub Actions** do automatyzacji procesów ciągłej integracji (CI). Skonfigurowany workflow automatycznie:
-   Buduje projekt (`dotnet build`) przy każdym pushu do repozytorium.


###  4.  Logowanie z NLog
Aplikacja posiada system logowania oparty o bibliotekę **NLog**. Konfiguracja pozwala na zapisywanie logów o różnym poziomie ważności (od śledzenia po błędy krytyczne) do plików tekstowych. Mechanizm logowania jest wstrzykiwany do serwisów i kontrolerów przez **Dependency Injection (`ILogger<T>`)**, co ułatwia monitorowanie oraz diagnostykę aplikacji.

![image](https://github.com/user-attachments/assets/33b77aa9-b65a-4ef9-af9f-60609516d9d0)

Logi odpowiedzialne:

![image](https://github.com/user-attachments/assets/7762808a-d471-40f3-95c8-c3a1dab57b35)

###  5. Usługi w Tle i Automatyczne Raportowanie
Zaimplementowałem mechanizm usług działających w tle (`BackgroundService`) do obsługi zadań cyklicznych. Przykładem jest usługa, która **raz dziennie generuje raport PDF** z listą otwartych zleceń serwisowych. Następnie, gotowy raport jest **automatycznie wysyłany jako załącznik e-mail** do administratora systemu, co pokazuje zdolność aplikacji do wykonywania autonomicznych, zaplanowanych operacji.

Przykładowy raport
![image](https://github.com/user-attachments/assets/c92c23f7-a2ee-4463-8f40-584367446cad)


###  6. Testy Wydajnościowe z NBomber
Przetestowałem, jak system radzi sobie pod obciążeniem, używając NBombera. Symulowałem wielu jednoczesnych użytkowników, którzy uderzali w jeden z kluczowych endpointów API. 



## Setup

1.  **Sklonuj repozytorium:**
    ```bash
    git clone [URL-do-repozytorium]
    cd [nazwa-katalogu]
    ```

2.  **Skonfiguruj Connection String:**
    Otwórz plik `appsettings.json` i zaktualizuj `DefaultConnection` do swojej instancji SQL Server.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=TWOJ_SERWER;Database=SystemZarz;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
    }
    ```

3.  **Uruchom aplikację:**
    ```bash
    dotnet run
    ```
    Aplikacja automatycznie:
    -   Zastosuje migracje i utworzy bazę danych, jeśli nie istnieje.
    -   Utworzy role: `Admin`, `User`, `Recepcjonista`, `Mechanik`.
    -   Stworzy domyślnego użytkownika **Admina** z danymi logowania:
        -   **Email:** `admin@1`
        -   **Hasło:** `Admin123!`

4.  **Przeglądaj API:**
    Przejdź pod adres `https://localhost:[PORT]/swagger`, aby zobaczyć interaktywną dokumentację API.

---


## Przykład jak działa aplikacja:
![image](https://github.com/user-attachments/assets/2dcaa9d3-d40a-405b-ab44-3489132f9525)
![image](https://github.com/user-attachments/assets/09aab209-b854-46c5-9c8e-b26de04f7f04)
![image](https://github.com/user-attachments/assets/54ecafda-4f8c-46c8-85f9-05ad4280e1b5)


Swagger - Przez autoryzacje nie można testować zapytań.


![image](https://github.com/user-attachments/assets/9b866058-d31e-46df-9670-e19ea5ed5d30)

Dziękuję za dotarcie do końca. Mam nadzieję, że projekt okazał się interesujący :)
