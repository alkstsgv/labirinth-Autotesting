using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;
using labirinthAutoTesting.Helpers;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ParametrizedFavoritesTests : BaseTest
{
	private CommonPageActions _commonPageActions = null!;
	public CommonPageActions CommonPageActions => _commonPageActions;
	private Helper _helper; public Helper Helper => _helper ??= new Helper(Page);

	[SetUp]
	public void SetupPageObjects()
	{
		_commonPageActions = new CommonPageActions(Page);
		_helper = new Helper(Page);
	}


	/// <summary>
	/// Тест-кейс 1: Добавление книги в «Отложено» по кнопке «Сердце» в блоке «Что почитать: выбор редакции».
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	[TestCase("/cabinet/putorder/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task TestCase1_VerifyBookAddedToFavListGeneric(string pagePath)
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
		string? trimmedTitleText = titleText?.Remove(titleText.Length - 1);
		await CommonPageActions.HeartIcon.ClickAsync();

		var popupAfterAddToFavList = Page.Locator("#minwidth .js-good-added");
		await Helper.WaitBetweenActions(popupAfterAddToFavList);
		await popupAfterAddToFavList.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(popupAfterAddToFavList);
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

	/// <summary>
	/// Тест-кейс 2: Переход в раздел «Отложено» через тултип в кнопке «Сердце».
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	[TestCase("/cabinet/putorder/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task TestCase2_VerifyPageWithAddedFavBooksGeneric(string pagePath)
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
		await CommonPageActions.OpenTooltip();
		await Page.Locator(".js-putorder-block-change .b-list-shell-item").Filter(new() { HasText = "Перейти к отложенным" }).ClickAsync();

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		await Expect(Page).ToHaveURLAsync("https://www.labirint.ru/cabinet/putorder/");
	}

	/// <summary>
	/// Тест-кейс 3: Удаление книги из раздела «Отложено» через тултип в кнопке «Сердце».
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	[TestCase("/cabinet/putorder/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task TestCase3_VerifyBookRemovedFromFavGeneric(string pagePath)
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
		await CommonPageActions.OpenTooltip();
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

	/// <summary>
	/// Тест-кейс 4: Закрытие тултипа после добавления товара в Отложенное.
	/// </summary>
	/// <param name="pagePath">Путь к странице (например, "/").</param>
	[TestCase("/")]
	[TestCase("/cabinet/putorder/")]
	[TestCase("/genres/2827/")]
	[TestCase("/school/")]
	public async Task TestCase4_CloseTooltip(string pagePath)
	{
		await Context.AddCookiesAsync(new[]
		{
			new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = pagePath },
		});

		Console.WriteLine("Тест-кейс 4. Общее. Закрытие тултипа после добавления товара в Отложенное");

		// === Подготовка теста === //
		Console.WriteLine("Выполнение предусловий");
		await GotoAsync(pagePath);
		var pageUrl = Page.Url;
		await CommonPageActions.AcceptModalWithCookies();

		// === Шаги === //
		Console.WriteLine("Проход по шагам тест-кейса");
		await CommonPageActions.OpenTooltip();
		var tooltip = Page.Locator(".js-putorder-block-change .b-dropdown-window-close");
		await tooltip.ClickAsync();
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

		// === Ожидаемый результат === //
		Console.WriteLine("Сверка ожидаемого результата");
		var pageUrlAfterMoves = Page.Url;
		await Expect(Page).ToHaveURLAsync(pageUrlAfterMoves);
		await Expect(tooltip).Not.ToBeVisibleAsync();

	}
}

