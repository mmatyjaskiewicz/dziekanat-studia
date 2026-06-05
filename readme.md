## Autor

Maksym Matyjaśkiewicz 15615 - lab2

## Zrealizowane funkcjonalności

### Uwierzytelnianie i autoryzacja

- logowanie użytkowników przy użyciu JWT
- odświeżanie tokenów (Refresh Token)
- wylogowanie użytkownika
- pobieranie danych aktualnie zalogowanego użytkownika
- autoryzacja oparta o role i polityki

### Zarządzanie studentami

- tworzenie studentów
- edycja danych studentów
- pobieranie listy studentów
- pobieranie szczegółów studenta
- zmiana statusu studenta
- przypisywanie studenta do programu studiów
- przypisywanie studenta do roku akademickiego
- paginacja wyników

### Zarządzanie wykładowcami

- tworzenie wykładowców
- edycja danych wykładowców
- pobieranie listy wykładowców
- pobieranie szczegółów wykładowcy
- pobieranie kursów prowadzonych przez wykładowcę
- pobieranie studentów przypisanych do wykładowcy

### Zarządzanie ocenami

- dodawanie ocen studentom
- pobieranie ocen studenta
- edycja ocen
- zapisywanie historii zmian ocen
- pobieranie historii zmian ocen

### Import danych

- import studentów z plików CSV
- import studentów z plików JSON
- walidacja importowanych danych
- raportowanie błędów importu

### Dodatkowe funkcjonalności

- walidacja numeru PESEL
- Repository Pattern
- Unit Of Work Pattern

## Wykorzystane technologie

- .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- ASP.NET Identity
- JWT Authentication

## Uruchomienie projektu

1. Sklonowanie repozytorium

git clone https://github.com/mmatyjaskiewicz/dziekanat-studia

2. Przywrócenie pakietów

dotnet restore

3. Utworzenie lub aktualizacja bazy danych

dotnet ef database update

4. Uruchomienie aplikacji

dotnet run

## Repozytorium GitHub

https://github.com/mmatyjaskiewicz/dziekanat-studia

## Dodatkowe informacje

Jako iż nie było mnie na wszystkich zajęciach zaimplementowałem 3 zadania zamiast 1, pozdrawiam.
