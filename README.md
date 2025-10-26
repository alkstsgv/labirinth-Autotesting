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

5. Запустите конкретный тест (например, по имени):
   ```
   dotnet test --filter "VerifyBookAddedToFavListGeneric"
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

4. Запустите конкретный тест:
   ```
   docker-compose run labirinth-tests --filter "VerifyBookAddedToFavListGeneric"
   ```

5. Для быстрой разработки (hot reload): После сборки монтируйте код и перекомпилируйте при изменениях:
   ```
   docker-compose run labirinth-tests dotnet build  # Перекомпилировать изменения
   docker-compose run labirinth-tests --filter "TestName"  # Запустить тесты
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
Вы можете управлять поведением тестов через переменные окружения в `docker-compose.yml`:
- **HEADLESS**: `true` (без GUI, по умолчанию) или `false` (с GUI для отладки).
- **SLOWMO**: Задержка между действиями в мс (по умолчанию 250; установите 0 для скорости).

Пример изменения в `docker-compose.yml`:
```yaml
environment:
  - HEADLESS=false
  - SLOWMO=0
```

После изменения пересоберите образ: `docker-compose build`.

## Режимы разработки
- **Dev (разработка)**: Используйте volume для hot reload. Изменяйте код локально, перекомпилируйте в контейнере и запускайте тесты без пересборки образа. Подходит для быстрой итерации. `docker-compose run` создаёт новые контейнеры каждый раз — используйте `--rm` для автоматического удаления после выполнения, или очищайте вручную.
  1. Соберите образ один раз: `docker-compose build`.
  2. При изменении кода: `docker-compose run --rm labirinth-tests dotnet build` (перекомпилировать).
  3. Запустите тесты: `docker-compose run --rm labirinth-tests dotnet test --filter "TestName"` (или все: `docker-compose up`).
  4. Очистите старые контейнеры: `docker-compose down --remove-orphans` или `docker system prune`.
- **Prod (production)**: Соберите образ один раз (`docker-compose build --no-cache`), запустите тесты в изолированной среде. Используйте headless=true и slowmo=0 для скорости. Подходит для CI/CD пайплайнов.

## Доступные тесты
- **ParametrizedFavoritesTests**: Параметризованные тесты для добавления/удаления книг в избранное на разных страницах (`/`, `/genres/2827/`, `/school/`).
- **BookPageTests**: Тесты для страницы конкретной книги (пока пустой; добавьте тесты для добавления в избранное на BookPage).

## Отладка
- Трассировка сохраняется в `trace.zip` (в контейнере — внутри `/app`).
- Для локальной отладки установите `HEADLESS=false` и `SLOWMO=250`.
- Логи выводятся в консоль.

## Расширение
- Добавляйте новые тесты в `Tests/`.
- Расширяйте классы в `Pages/` и `TestBase/` для новых страниц/действий.
- Обновляйте селекторы в коде при изменениях на сайте.

Если возникнут вопросы, проверьте логи или обратитесь к документации Playwright.