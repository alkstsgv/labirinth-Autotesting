# Labirinth AutoTesting Framework

Этот проект представляет собой фреймворк для UI-автотестов на C# с использованием Playwright и NUnit. Он предназначен для тестирования функционала сайта "Лабиринт" (добавление книг в избранное, навигация и т.д.). Фреймворк следует паттерну Page Object Model (POM) для поддерживаемости и переиспользования кода.

## Структура проекта
- **Pages/**: Классы страниц (BasePage, BookPage) — инкапсулируют элементы и действия на страницах.
- **TestBase/**: Базовые классы (BaseTest, CommonPageActions) — настройка браузера, общие методы для тестов.
- **Tests/**: Тестовые классы (ParametrizedFavoritesTests, BookPageTests) — сами тесты с проверками.
- **Dockerfile**: Для сборки Docker-образа.
- **docker-compose.yml**: Для запуска контейнера с ограничениями ресурсов.

## Требования
- .NET 8.0
- Docker (для контейнеризации)
- Playwright (автоматически устанавливается через NuGet)

## Установка и запуск

### Локальный запуск
1. Склонируйте репозиторий:
   ```
   git clone <url-репозитория>
   cd labirinthAutoTesting
   ```

2. Восстановите зависимости:
   ```
   dotnet restore
   ```

3. Соберите проект:
   ```
   dotnet build
   ```

4. Запустите все тесты:
   ```
   dotnet test
   ```

5. Запустите все тесты из файла (класса) BookPageTests.cs:
   ```
   dotnet test --filter "FullyQualifiedName~BookPageTests"
   ```

6. Запустите все тесты из папки Tests/:
   ```
   dotnet test --filter "TestNamespace=labirinthAutoTesting.Tests"
   ```

### Запуск в Docker
1. Склонируйте репозиторий и перейдите в папку проекта.

2. Соберите образ:
   ```
   docker-compose build
   ```

3. Запустите все тесты (headless по умолчанию):
   ```
   docker-compose up
   ```

4. Запустите все тесты:
   ```
   docker-compose run labirinth-tests dotnet test labirinthAutoTesting.csproj
   ```

5. Запустите все тесты из файла (класса) BookPageTests.cs:
   ```
   docker-compose run labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "FullyQualifiedName~BookPageTests"
   ```

6. Запустите все тесты из папки Tests/:
   ```
   docker-compose run labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "TestNamespace=labirinthAutoTesting.Tests"
   ```

5. Для быстрой разработки (hot reload): После сборки монтируйте код и перекомпилируйте при изменениях:
   ```
   docker-compose run labirinth-tests dotnet build  # Перекомпилировать изменения
   docker-compose run labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "FullyQualifiedName~BookPageTests"  # Запустить тесты из BookPageTests.cs
   docker-compose run labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "TestNamespace=labirinthAutoTesting.Tests"  # Запустить все тесты из папки Tests/
   ```
   Изменения в коде сразу отражаются благодаря volume в `docker-compose.yml`.

6. Остановите и удалите контейнеры/образы после использования:
   ```
   docker-compose down  # Остановить контейнеры
   docker-compose down --volumes --remove-orphans  # Очистить volumes и orphaned контейнеры
   docker system prune -a  # Удалить неиспользуемые образы и контейнеры (осторожно, удалит все!)
   ```
   Для удаления конкретного образа: `docker rmi labirinth-tests`.

## Управление параметрами
Вы можете управлять поведением тестов через переменные окружения:
- **HEADLESS**: `true` (без GUI, по умолчанию) или `false` (с GUI для отладки).
- **SLOWMO**: Задержка между действиями в мс (по умолчанию 0; установите 250 для отладки).

### В Docker
Установите переменные в `docker-compose.yml` (но они переопределяются `.env` файлами):
```yaml
environment:
  - HEADLESS=false
  - SLOWMO=250
```
После изменения пересоберите образ: `docker-compose build`.

### Локально (вне Docker)
При работе вне Docker значения берутся из файлов `.env` или `.env_orig` (если `.env` отсутствует). Создайте `.env` в корне проекта:
```
HEADLESS=false
SLOWMO=250
```
- `.env` имеет приоритет над `.env_orig`.
- Если файлы отсутствуют, используются значения по умолчанию (HEADLESS=true, SLOWMO=0).

## Режимы разработки
- **Dev (разработка)**: Используйте volume для hot reload. Изменяйте код локально, перекомпилируйте в контейнере и запускайте тесты без пересборки образа. Подходит для быстрой итерации. `docker-compose run` создаёт новые контейнеры каждый раз — используйте `--rm` для автоматического удаления после выполнения, или очищайте вручную.
  1. Соберите образ один раз: `docker-compose build`.
  2. При изменении кода: `docker-compose run --rm labirinth-tests dotnet build` (перекомпилировать).
  3. Запустите все тесты: `docker-compose run --rm labirinth-tests dotnet test labirinthAutoTesting.csproj`.
  4. Запустите все тесты из файла (класса) BookPageTests.cs: `docker-compose run --rm labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "FullyQualifiedName~BookPageTests"`.
  5. Запустите все тесты из папки Tests/: `docker-compose run --rm labirinth-tests dotnet test labirinthAutoTesting.csproj --filter "TestNamespace=labirinthAutoTesting.Tests"`.
  6. Очистите старые контейнеры: `docker-compose down --remove-orphans` или `docker system prune`.
  7. Просмотр логов трассировки: Скачайте `trace.zip` из контейнера (`docker cp <container_id>:/app/trace.zip .`) и откройте в Playwright Trace Viewer (`playwright show-trace trace.zip`).
- **Prod (production)**: Соберите образ один раз (`docker-compose build --no-cache`), запустите тесты в изолированной среде. Используйте headless=true и slowmo=0 для скорости. Подходит для CI/CD пайплайнов.

## Доступные тесты
- **ParametrizedFavoritesTests**: Параметризованные тесты для добавления/удаления книг в избранное на разных страницах (`/`, `/genres/2827/`, `/school/`).
  - [Тест-кейс 1](Wiki/TestCases/TestCase1.md): Общее. Добавление книги в «Отложено» по кнопке «Сердце» в блоке «Что почитать: выбор редакции».
  - [Тест-кейс 2](Wiki/TestCases/TestCase2.md): Общее. Переход в раздел «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции».
  - [Тест-кейс 3](Wiki/TestCases/TestCase3.md): Общее. Удаление книги из раздела «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции».
  - [Тест-кейс 4](Wiki/TestCases/TestCase4.md): Общее. Закрытие тултипа после добавления товара в Отложенное.
- **BookPageTests**: Тесты для страницы конкретной книги.
  - [Тест-кейс 5](Wiki/TestCases/TestCase5.md): Карточка книги. Добавление в Отложенное.
  - [Тест-кейс 6](Wiki/TestCases/TestCase6.md): Карточка книги. Удаление добавленной книги из избранного.
  - [Тест-кейс 7](Wiki/TestCases/TestCase7.md): Карточка книги. Добавление книги в Отложенное из блока с рекомендациями «Книги из жанра».
  - [Тест-кейс 8](Wiki/TestCases/TestCase8.md): Карточка книги. Удаление добавленной книги из блока с рекомендациями «Книги из жанра» из «Отложенное».

## Отладка
- Трассировка сохраняется в `trace.zip` (в контейнере — внутри `/app`).
- Для просмотра логов трассировки: `playwright show-trace trace.zip` (требуется установка Playwright CLI: `dotnet tool install --global Microsoft.Playwright.CLI`).
- Для локальной отладки установите `HEADLESS=false` и `SLOWMO=250`.
- Логи выводятся в консоль.

## Расширение
- Добавляйте новые тесты в `Tests/`.
- Расширяйте классы в `Pages/` и `TestBase/` для новых страниц/действий.
- Обновляйте селекторы в коде при изменениях на сайте.

## Документация тест-кейсов

Подробные описания тест-кейсов доступны в [Wiki](Wiki/README.md).

Если возникнут вопросы, проверьте логи или обратитесь к документации Playwright.