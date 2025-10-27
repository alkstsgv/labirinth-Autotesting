using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.VisualBasic;

using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class BookPageTests : BaseTest
{
	private BookPage _bookPage = null!;
	public BookPage BookPage => _bookPage;
	private CommonPageActions _commonPageActions = null!;
	public CommonPageActions CommonPageActions => _commonPageActions;
	

	[SetUp]
	public void SetupPageObjects()
	{
		_bookPage = new BookPage(Page);
		_commonPageActions = new CommonPageActions(Page);
	}

	[TestCase("/")]
	public async Task AddBookToFavFromBookPage(string pagePath)
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
		await BookPage.ClickProductHeartIconButton();
		await BookPage.ClickProductHeartIconButton();
		var heartInNavbar = Page.GetByLabel("Перейти в раздел отложенных товаров").Locator("span").Nth(1);
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();
		var imgProductHeartIcon = BookPage.ProductHeartIconButton.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-red");
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}	

	[TestCase("/")]
	public async Task RemoveBookFromFavFromList (string pagePath)
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
		await BookPage.ClickProductHeartIconButton();
		await BookPage.ClickProductHeartIconButton();
		await BookPage.ClickProductHeartIconButton();
		await BookPage.ClickProductHeartIconButton();
		var heartInNavbar = Page.GetByLabel("Перейти в раздел отложенных товаров").Locator("span").Nth(1);
		var imgProductHeartIcon = BookPage.ProductHeartIconButton.Locator("img");

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(imgProductHeartIcon).ToHaveAttributeAsync("alt", "heart-outline-gray-700");
		Assert.That(await heartInNavbar.IsVisibleAsync(), Is.False);
	}	

}
