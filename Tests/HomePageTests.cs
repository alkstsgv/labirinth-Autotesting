using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;

using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;


namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class HomePageTester : BaseTest
{
	private HomePage _homePage = null!;
	// private IPage _page = null!;
	private PlaywrightTest _playwright = new PlaywrightTest();

	[SetUp]
	public void SetupPageObjects()
	{
		_homePage = new HomePage(Page);
	}

	[Test]
	public async Task CheckAddBookToFavList()
	{
		Console.WriteLine("Тест-кейс 1. Добавление книги в «Отложено» по кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync("/");
		await _homePage.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await _homePage.AddBookToFavList();
		var bookTitle = _homePage._firstBookInTheRow.Locator(".product-cover-long .cover").First;
		string? titleText = await bookTitle.GetAttributeAsync("href");
		string? trimmedTitleText = titleText.Remove(titleText.Length - 1);


		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var element = _homePage._favoriteButton;
		var popupAfterAddToFavList = Page.Locator("#minwidth .js-good-added");
		var popupBookTitle = popupAfterAddToFavList.Locator(".b-basket-popinfo-e-text-m-add a[href]");
		string? popupBookTitleName = await popupBookTitle.GetAttributeAsync("href");
		var heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder");
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();
		Console.WriteLine(heartInNavbarNumbers);

		await _playwright.Expect(element).ToBeAttachedAsync();
		await _playwright.Expect(element).ToBeVisibleAsync();
		await _playwright.Expect(element).ToHaveCSSAsync("color", "rgb(173, 10, 5)");
		await _playwright.Expect(popupAfterAddToFavList).ToBeAttachedAsync();
		await _playwright.Expect(popupAfterAddToFavList).ToBeVisibleAsync();
		await _playwright.Expect(popupAfterAddToFavList).ToBeInViewportAsync();
		Assert.That(popupBookTitleName, Is.EqualTo(trimmedTitleText));

		await _playwright.Expect(heartInNavbar).ToBeAttachedAsync();
		await _playwright.Expect(heartInNavbar).ToBeVisibleAsync();
		await _playwright.Expect(heartInNavbar).ToBeInViewportAsync();
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

}
