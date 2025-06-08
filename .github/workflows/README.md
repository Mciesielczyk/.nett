Ten plik automatycznie sprawdza nasz projekt System_Zarz za każdym razem, gdy:

- Ktoś doda nowy kod do brancha Projekt
- Ktoś zrobi pull request
- Uruchomimy to ręcznie

1. Pobiera kod z repozytorium
2. Ustawia .NET 8.0 na serwerze
3. Przechodzi do folderu System_Zarz
4. Pobiera wszystkie potrzebne biblioteki (dotnet restore)
5. Kompiluje projekt - sprawdza czy kod się buduje bez błędów
6. Uruchamia testy z folderu MyProject.Tests
