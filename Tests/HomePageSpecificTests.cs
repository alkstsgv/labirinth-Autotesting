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

	[SetUp]
	public void SetupPageObjects()
	{
		_homePage = new HomePage(Page);
	}

	[TestCase("/")]
	public async Task CheckAddBookToFavList(string pagePath)
	{

		await Context.AddCookiesAsync(new[]
        {
            new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
        });

		Console.WriteLine("Тест-кейс 1. Добавление книги в «Отложено» по кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await _homePage.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await _homePage.AddBookToFavList();
		var bookTitle = _homePage.FirstBookInTheRow.Locator(".product-cover-long .cover").First;
		string? titleText = await bookTitle.GetAttributeAsync("href");
		string? trimmedTitleText = titleText.Remove(titleText.Length - 1);


		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var element = _homePage.FavoriteButton;
		var popupAfterAddToFavList = Page.Locator("#minwidth .js-good-added");
		var popupBookTitle = popupAfterAddToFavList.Locator(".b-basket-popinfo-e-text-m-add a[href]");
		string? popupBookTitleName = await popupBookTitle.GetAttributeAsync("href");
		var heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();

		await Expect(element).ToBeAttachedAsync();
		await Expect(element).ToBeVisibleAsync();
		await Expect(element).ToHaveCSSAsync("color", "rgb(173, 10, 5)");

		await Expect(popupAfterAddToFavList).ToBeAttachedAsync();
		await Expect(popupAfterAddToFavList).ToBeVisibleAsync();
		await Expect(popupAfterAddToFavList).ToBeInViewportAsync();
		Assert.That(popupBookTitleName, Is.EqualTo(trimmedTitleText));

		await Expect(heartInNavbar).ToBeAttachedAsync();
		await Expect(heartInNavbar).ToBeVisibleAsync();
		await Expect(heartInNavbar).ToBeInViewportAsync();
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

}
