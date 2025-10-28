using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.VisualBasic;

using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;
using labirinthAutoTesting.Helpers;
using System.Data;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class BookPageTests : BaseTest
{
	private BookPage _bookPage = null!;
	public BookPage BookPage => _bookPage;
	private CommonPageActions _commonPageActions = null!;
	public CommonPageActions CommonPageActions => _commonPageActions;
	private Helper _helper; public Helper Helper => _helper ??= new Helper(Page);


	[SetUp]
	public void SetupPageObjects()
	{
		_bookPage = new BookPage(Page);
		_commonPageActions = new CommonPageActions(Page);
		_helper = new Helper(Page);
	}

	/// <summary>
	/// Тест-кейс 5: Добавление книги в избранное со страницы книги.
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	public async Task TestCase5_AddBookToFavFromBookPage(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 5. Карточка книги. Добавление в Отложенное");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync("/");
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await BookPage.ClickFirstBookOnPage();
		await BookPage.OneClickProductHeartButton();
		var heartInNavbar = BookPage.ProductHeartInNavbar.Locator("span").Nth(1);
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();
		var imgProductHeartIcon = BookPage.ProductHeartIconButton.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-red");
		Assert.That(await heartInNavbar.IsVisibleAsync(), Is.True);
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

	/// <summary>
	/// Тест-кейс 6: Удаление добавленной книги из избранного со страницы книги.
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	public async Task TestCase6_RemoveBookFromFavFromList(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 6. Удаление добавленной книги из избранного");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync("/");
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await BookPage.ClickFirstBookOnPage();
		await BookPage.DoubleClickProductHeartButton();
		var heartInNavbar = BookPage.ProductHeartInNavbar.Locator("span").Nth(1);
		var imgProductHeartIcon = BookPage.ProductHeartIconButton.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-outline-gray-700");
		Assert.That(await heartInNavbar.IsVisibleAsync(), Is.False);
	}

	/// <summary>
	/// Тест-кейс 7: Добавление книги в избранное из блока рекомендаций "Книги из жанра".
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	public async Task TestCase7_AddBookToFavFromRecList(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 7. Добавление книги в Отложенное из блока с рекомендациями «Книги из жанра» ");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync("/");
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await BookPage.ClickFirstBookOnPage();
		await BookPage.OneClickProductHeartInRecRow();
		var heartInNavbar = BookPage.ProductHeartInNavbar.Locator("span").Nth(1);
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();
		var imgProductHeartIcon = BookPage.ProductListHeart.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-red");
		Assert.That(await heartInNavbar.IsVisibleAsync(), Is.True);
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

	/// <summary>
	/// Тест-кейс 8: Удаление добавленной книги из блока рекомендаций "Книги из жанра" из избранного.
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	public async Task TestCase8_RemoveAddedBookFromFromRecList(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 8. Удаление добавленной книги из блока с рекомендациями «Книги из жанра» из «Отложенное»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync("/");
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await BookPage.ClickFirstBookOnPage();
		await BookPage.DoubleClickProductHeartInRecRow();
		var heartInNavbar = BookPage.ProductHeartInNavbar.Locator("span").Nth(1);
		var imgProductHeartIcon = BookPage.ProductListHeart.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-outline-gray-700");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-outline-gray-700");
		Assert.That(await heartInNavbar.IsVisibleAsync(), Is.False);
	}
}
