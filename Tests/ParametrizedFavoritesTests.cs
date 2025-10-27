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
public class ParametrizedFavoritesTests : BaseTest
{
	private CommonPageActions _commonPageActions = null!;
	public CommonPageActions CommonPageActions => _commonPageActions;

	[SetUp]
	public void SetupPageObjects()
	{
		_commonPageActions = new CommonPageActions(Page);
	}


	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task VerifyBookAddedToFavListGeneric(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 1. Общее. Добавление книги в «Отложено» по кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await CommonPageActions.GetFirstHeartOnPage();
		await CommonPageActions.CheckHeartIconStatus();
		var productCard = CommonPageActions.HeartIcon.Locator("xpath=ancestor::div[contains(@class, 'product')]");
		var bookTitle = productCard.Locator("a.cover").First;
		string? titleText = await bookTitle.GetAttributeAsync("href");
		string? trimmedTitleText = titleText.Remove(titleText.Length - 1);
		await CommonPageActions.HeartIcon.ClickAsync();

		var popupAfterAddToFavList = Page.Locator("#minwidth .js-good-added");
		await popupAfterAddToFavList.IsVisibleAsync();
		await popupAfterAddToFavList.IsEnabledAsync();
		await popupAfterAddToFavList.ScrollIntoViewIfNeededAsync();
		var popupBookTitle = popupAfterAddToFavList.Locator(".b-basket-popinfo-e-text-m-add a[href]");
		string? trimmedPopupBookTitleName = await popupBookTitle.GetAttributeAsync("href");

		await CommonPageActions.GetHeartInNavbar();
		string? heartInNavbarNumbers = await CommonPageActions.HeartInNavbar.InnerTextAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(CommonPageActions.HeartIcon).ToBeAttachedAsync();
		await Expect(CommonPageActions.HeartIcon).ToBeVisibleAsync();
		await Expect(CommonPageActions.HeartIcon).ToHaveClassAsync(new Regex(@"\bactive\b"));

		await Expect(popupAfterAddToFavList).ToBeAttachedAsync();
		await Expect(popupAfterAddToFavList).ToBeVisibleAsync();
		await Expect(popupAfterAddToFavList).ToBeInViewportAsync();

		Assert.That(trimmedPopupBookTitleName, Is.EqualTo(trimmedTitleText));

		await Expect(CommonPageActions.HeartInNavbar).ToBeAttachedAsync();
		await Expect(CommonPageActions.HeartInNavbar).ToBeVisibleAsync();
		await Expect(CommonPageActions.HeartInNavbar).ToBeInViewportAsync();

		Assert.That(heartInNavbarNumbers, Is.EqualTo("1"));
	}

	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task VerifyPageWithAddedFavBooksGeneric(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 2. Общее. Переход в раздел «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await CommonPageActions.GetFirstHeartOnPage();
		await CommonPageActions.CheckHeartIconStatus();
		await CommonPageActions.DoubleHeartIconClick();
		await Page.Locator(".js-putorder-block-change .b-list-shell-item").Filter(new() { HasText = "Перейти к отложенным" }).ClickAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(Page).ToHaveURLAsync("https://www.labirint.ru/cabinet/putorder/");
	}

	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task VerifyBookRemovedFromFavGeneric(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 3. Общее. Удаление книги из раздела «Отложено» через тултип в кнопке «Сердце» в блоке «Что почитать: выбор редакции»");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await CommonPageActions.GetFirstHeartOnPage();
		await CommonPageActions.CheckHeartIconStatus();
		await CommonPageActions.DoubleHeartIconClick();
		await Page.Locator(".js-putorder-block-change .b-list-shell-item").Filter(new() { HasText = "Убрать из отложенных" }).ClickAsync();
		await CommonPageActions.CheckHeartInNavbarStatus();
		string? heartInNavbarNumbers = await CommonPageActions.HeartInNavbar.InnerTextAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(CommonPageActions.HeartIcon).ToBeAttachedAsync();
		await Expect(CommonPageActions.HeartIcon).ToBeVisibleAsync();
		await Expect(CommonPageActions.HeartIcon).Not.ToHaveClassAsync(new Regex(@"\bactive\b"));

		await Expect(CommonPageActions.HeartInNavbar).ToBeAttachedAsync();
		await Expect(CommonPageActions.HeartInNavbar).ToBeVisibleAsync();
		await Expect(CommonPageActions.HeartInNavbar).ToBeInViewportAsync();

		Assert.That(heartInNavbarNumbers, Is.EqualTo("0"));
	}

	[TestCase("/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task CloseTooltip(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 4. Общее. Закрытие тултипа после добавления товара в Отложенное");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		Console.WriteLine("Проход по шагам тест-кейса");
		await CommonPageActions.GetFirstHeartOnPage();
		await CommonPageActions.CheckHeartIconStatus();
		await CommonPageActions.DoubleHeartIconClick();
		var tooltip = Page.Locator(".js-putorder-block-change .b-dropdown-window-close");
		await tooltip.ClickAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var pageUrl = Page.Url;
		Console.WriteLine(pageUrl);
		await Expect(tooltip).Not.ToBeVisibleAsync();

	}
}

