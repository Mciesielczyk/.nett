![image](https://github.com/user-attachments/assets/ce7b5794-0a3e-42c9-b68a-f18834b012ed)


async / await	- Pobieranie tekstów z sieci przez HttpClient, nie blokując głównego wątku
Task.WhenAll()	- Równoległe pobieranie wszystkich plików jednocześnie
Parallel.ForEach	- Równoległe przetwarzanie tekstów (zliczanie słów)
ConcurrentDictionary	- Synchronizacja i bezpieczny zapis danych z wielu wątków
Stopwatch -	Pomiar czasu pobierania i przetwarzania danych

1.
Task.WhenAll
– do asynchronicznego pobierania wielu plików jednocześnie (HttpClient.GetAsync).
Dzięki temu zamiast czekać na każdy plik osobno, pobierane są wszystkie naraz.

Parallel.ForEach lub Task.Run
– do równoległego przetwarzania tekstów (czyli zliczania słów).
Każdy tekst jest przetwarzany w osobnym wątku/tasku.

2.
Dlaczego synchronizacja była konieczna?
Gdy wiele wątków jednocześnie modyfikuje wspólną kolekcję może dojść do
konfliktów (dwa wątki próbują dodać lub zmienić ten sam klucz),
niespójnych danych (np. słowo zliczone tylko raz, mimo że pojawiło się wiele razy)

3.
Jak wyglądałby ten kod bez równoległości?
wszystkie teksty byłyby pobierane jeden po drugim (czyli await Pobierz(...) po kolei),
każde przetwarzanie byłoby wykonywane jeden po drugim, w jednej głównej pętli,
nie trzeba by było synchronizować dostępu do słownika.

Zmienilby sie czas.

4.
Jak można jeszcze poprawić wydajność?
Nie da sie.
