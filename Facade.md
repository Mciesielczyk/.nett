# Wzorzec Projektowy: Facade

## Opis

Wzorzec projektowy **Facade** polega na stworzeniu uproszczonego interfejsu do złożonego systemu, który składa się z wielu podsystemów. Umożliwia on klientowi łatwiejszą interakcję z systemem, ukrywając szczegóły implementacyjne.

### Główne założenia:
- Ukrywa złożoność systemu.
- Uproszcza interakcję z wieloma podsystemami.
- Umożliwia łatwiejszą obsługę złożonych procesów w dużych systemach.

## Problem, który rozwiązuje

W systemach składających się z wielu podsystemów, często konieczne jest komunikowanie się z różnymi modułami, które mają własne interfejsy. Wzorzec **Facade** redukuje tę złożoność, zapewniając prosty interfejs do zarządzania wszystkimi modułami systemu. Dzięki temu, klient może używać jednego obiektu do zarządzania złożonymi procesami.

## Schemat UML


![image](https://github.com/user-attachments/assets/23a79548-3697-4f2a-a59b-0a953643ba2b)

1. Klienci (Client A, Client B, Client C) chcą skorzystać z funkcjonalności podsystemu.
2. Zamiast bezpośrednio wchodzić w interakcję z klasami w podsystemie, klienci korzystają z Fasady.
3. Fasada odbiera wywołanie od klienta i deleguje je do odpowiednich klas w podsystemie.
4. Klasy w podsystemie wykonują operacje i zwracają wynik do Fasady.
5. Fasada zwraca wynik do klienta.


## Przykładowa Implementacja w C#

```csharp
// Podsystem 1
public class LightingSystem
{
    public void TurnOnLights() => Console.WriteLine("Lights are turned on.");
    public void TurnOffLights() => Console.WriteLine("Lights are turned off.");
}

// Podsystem 2
public class HeatingSystem
{
    public void TurnOnHeating() => Console.WriteLine("Heating is turned on.");
    public void TurnOffHeating() => Console.WriteLine("Heating is turned off.");
}

// Facade
public class HomeFacade
{
    private LightingSystem _lightingSystem = new LightingSystem();
    private HeatingSystem _heatingSystem = new HeatingSystem();

    public void TurnOnEverything()
    {
        _lightingSystem.TurnOnLights();
        _heatingSystem.TurnOnHeating();
    }

    public void TurnOffEverything()
    {
        _lightingSystem.TurnOffLights();
        _heatingSystem.TurnOffHeating();
    }
}

// Klient
public class Client
{
    public static void Main(string[] args)
    {
        HomeFacade facade = new HomeFacade();
        
        // Klient może używać jednego interfejsu
        facade.TurnOnEverything();
        facade.TurnOffEverything();
    }
}


```
**Plusy**:
- Uproszczenie interfejsu.
- Ukrywanie złożoności systemu.
- Lepsza organizacja kodu.
- Elastyczność i łatwość testowania.

**Minusy**:

1. **Zwiększenie zależności**
   - Zastosowanie facady może spowodować, że cała aplikacja będzie zależna od jednej klasy facady, co może ograniczyć elastyczność w dalszym rozwoju systemu. Zbyt duża zależność od facady może prowadzić do problemów z rozbudową systemu w przyszłości.

2. **Ograniczenie dostępu do funkcji podsystemów**
   - Facada upraszcza interfejs, ale przez to może ograniczyć dostęp do niektórych funkcji podsystemów. Jeśli klient potrzebuje dostępu do specyficznych metod podsystemów, musi go szukać poza facadą, co może prowadzić do konieczności obejścia wzorca.

3. **Przeciążenie facady**
   - Facada może stać się zbyt dużą klasą, jeśli będzie musiała obsługiwać zbyt wiele funkcji. Może się wtedy stać punktem centralnym, który jest zbyt skomplikowany i trudny w utrzymaniu. Istnieje ryzyko, że facada stanie się za bardzo rozbudowana, a jej zadanie – zarządzanie prostym interfejsem – zostanie utrudnione.

4. **Potrzebna jest dodatkowa warstwa**
   - Wzorzec Facade dodaje kolejną warstwę abstrakcji w systemie. Choć w wielu przypadkach jest to zaleta, w niektórych sytuacjach ta dodatkowa warstwa może sprawić, że system będzie bardziej skomplikowany i mniej przejrzysty.


---
