using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;


namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ParametrizedFavoritesTester : BaseTest
{
	private HomePage _homePage = null!;

	[SetUp]
	public void SetupPageObjects()
	{
		_homePage = new HomePage(Page);
	}


	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task VerifyBookAddedToFavListGeneric (string pagePath)
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
		var heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await heartIcon.IsVisibleAsync();
		await heartIcon.IsEnabledAsync();
		await heartIcon.ScrollIntoViewIfNeededAsync();
		var productCard = heartIcon.Locator("xpath=ancestor::div[contains(@class, 'product')]");
		var bookTitle = productCard.Locator("a.cover").First;
		string? titleText = await bookTitle.GetAttributeAsync("href");
		string? trimmedTitleText = titleText.Remove(titleText.Length - 1);
		await heartIcon.ClickAsync();


		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var popupAfterAddToFavList = Page.Locator("#minwidth .js-good-added");
		await popupAfterAddToFavList.IsVisibleAsync();
		await popupAfterAddToFavList.IsEnabledAsync();
		await popupAfterAddToFavList.ScrollIntoViewIfNeededAsync();
		var popupBookTitle = popupAfterAddToFavList.Locator(".b-basket-popinfo-e-text-m-add a[href]");
		string? trimmedPopupBookTitleName = await popupBookTitle.GetAttributeAsync("href");

		var heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		await heartInNavbar.IsVisibleAsync();
		await heartInNavbar.IsEnabledAsync();
		await heartInNavbar.ScrollIntoViewIfNeededAsync();
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();

		await Expect(heartIcon).ToBeAttachedAsync();
		await Expect(heartIcon).ToBeVisibleAsync();
		await Expect(heartIcon).ToHaveCSSAsync("color", "rgb(173, 10, 5)");

		await Expect(popupAfterAddToFavList).ToBeAttachedAsync();
		await Expect(popupAfterAddToFavList).ToBeVisibleAsync();
		await Expect(popupAfterAddToFavList).ToBeInViewportAsync();
		Assert.That(trimmedPopupBookTitleName, Is.EqualTo(trimmedTitleText));

		await Expect(heartInNavbar).ToBeAttachedAsync();
		await Expect(heartInNavbar).ToBeVisibleAsync();
		await Expect(heartInNavbar).ToBeInViewportAsync();
		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task VerifyPageWithAddedFavBooksGeneric (string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 2. Переход в раздел «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await _homePage.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		var heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await heartIcon.IsVisibleAsync();
		await heartIcon.IsEnabledAsync();
		await heartIcon.ScrollIntoViewIfNeededAsync();
		await heartIcon.ClickAsync();
		await heartIcon.ClickAsync();
		await Page.Locator(".js-putorder-block-change .b-list-shell-item").Filter(new() { HasText = "Перейти к отложенным" }).ClickAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(Page).ToHaveURLAsync("https://www.labirint.ru/cabinet/putorder/");
	}

	[TestCase("/")]
	// [TestCase("/genres/2827/")]
	// [TestCase("/school/")]
	public async Task VerifyBookRemovedFromFavGeneric(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 3. Удаление книги из раздела «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await _homePage.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		var heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await heartIcon.IsVisibleAsync();
		await heartIcon.IsEnabledAsync();
		// await heartIcon.ScrollIntoViewIfNeededAsync();
		await heartIcon.ClickAsync();
		await heartIcon.ClickAsync();
		await Page.Locator(".js-putorder-block-change .b-list-shell-item").Filter(new() { HasText = "Убрать из отложенных" }).ClickAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		await heartInNavbar.IsVisibleAsync();
		await heartInNavbar.IsEnabledAsync();
		// await heartInNavbar.ScrollIntoViewIfNeededAsync();
		string? heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();

		await Expect(heartIcon).ToBeAttachedAsync();
		await Expect(heartIcon).ToBeVisibleAsync();
		await Page.Mouse.MoveAsync(0, 0);
		await Expect(heartIcon).ToHaveCSSAsync("color", "rgb(24, 104, 160)");

		await Expect(heartInNavbar).ToBeAttachedAsync();
		await Expect(heartInNavbar).ToBeVisibleAsync();
		await Expect(heartInNavbar).ToBeInViewportAsync();
		Assert.That(heartInNavbarNumbers, Is.EqualTo("0"));
	}
}
