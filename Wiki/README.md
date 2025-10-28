# Тест-кейсы для тестирования функционала избранных товаров

Здесь собраны все тест-кейсы для проверки работы избранных товаров (отложенных книг) на сайте Labirint.ru.

## Список тест-кейсов

- [Тест-кейс 1: Добавление книги в «Отложено» на главной странице](TestCases/TestCase1.md) — реализовано в [`ParametrizedFavoritesTests.VerifyBookAddedToFavListGeneric`](../Tests/ParametrizedFavoritesTests.cs)
- [Тест-кейс 2: Переход в раздел «Отложено» через тултип](TestCases/TestCase2.md) — реализовано в [`ParametrizedFavoritesTests.VerifyPageWithAddedFavBooksGeneric`](../Tests/ParametrizedFavoritesTests.cs)
- [Тест-кейс 3: Удаление книги из «Отложено» через тултип](TestCases/TestCase3.md) — реализовано в [`ParametrizedFavoritesTests.VerifyBookRemovedFromFavGeneric`](../Tests/ParametrizedFavoritesTests.cs)
- [Тест-кейс 4: Закрытие тултипа после добавления товара](TestCases/TestCase4.md) — реализовано в [`ParametrizedFavoritesTests.CloseTooltip`](../Tests/ParametrizedFavoritesTests.cs)
- [Тест-кейс 5: Добавление книги в «Отложено» на странице книги](TestCases/TestCase5.md) — реализовано в [`BookPageTests.TestCase5_AddBookToFavFromBookPage`](../Tests/BookPageTests.cs)
- [Тест-кейс 6: Удаление книги из избранного на странице книги](TestCases/TestCase6.md) — реализовано в [`BookPageTests.TestCase6_RemoveBookFromFavFromList`](../Tests/BookPageTests.cs)
- [Тест-кейс 7: Добавление книги в «Отложено» из блока рекомендаций «Книги из жанра»](TestCases/TestCase7.md) — реализовано в [`BookPageTests.TestCase7_AddBookToFavFromRecList`](../Tests/BookPageTests.cs)
- [Тест-кейс 8: Удаление книги из блока рекомендаций «Книги из жанра» из «Отложенное»](TestCases/TestCase8.md) — реализовано в [`BookPageTests.TestCase8_RemoveAddedBookFromFromRecList`](../Tests/BookPageTests.cs)

## Структура проекта

- `TestCases/` - папка с описаниями тест-кейсов в формате Markdown.
- Каждый файл содержит предусловия, шаги и ожидаемый результат.

## Запуск тестов

Тесты реализованы с использованием Playwright и NUnit. Для запуска используйте:

```bash
dotnet test
```

Или для конкретного теста:

```bash
dotnet test --filter "TestName"