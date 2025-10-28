using System.Text.RegularExpressions;
using labirinthAutoTesting.TestBase;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using labirinthAutoTesting.Helpers;

namespace labirinthAutoTesting.Pages;

public class BookPage : BasePage
{
	private ILocator _productHeartIconButton = null!;
	public ILocator ProductHeartIconButton => _productHeartIconButton ??= Page.Locator(".items-center.cursor-pointer button");
	private ILocator _productHeartInNavbar = null!;
	public ILocator ProductHeartInNavbar => _productHeartInNavbar ??= Page.GetByLabel("Перейти в раздел отложенных товаров");
	private ILocator _productCard = null!;
	public ILocator ProductCard => _productCard ??= Page.Locator("a.cover:has(span.default-cover)");
	private ILocator _productListHeart = null!;
	public ILocator ProductListHeart => _productListHeart;

	private Helper _helper;
	public Helper Helper => _helper ??= new Helper(Page);

	public BookPage(IPage page) : base(page)
	{
		_helper = new Helper(page);
	}

	/// <summary>
	/// Кликает на первую книгу на странице, ожидая загрузки DOM.
	/// </summary>
	public async Task ClickFirstBookOnPage()
	{
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await ProductCard.First.ClickAsync();
	}
	/// <summary>
	/// Кликает на кнопку сердца для добавления/удаления книги из избранного, ожидая видимости элементов.
	/// </summary>
	public async Task ClickProductHeartIconButton()
	{
		var favPop = Page.Locator("section.area-price");
		await Helper.WaitBetweenActions(favPop, isVisible: true, isAttached: true);
		var cityInFavPop = favPop.Locator(".gap-6");
		await Helper.WaitBetweenActions(cityInFavPop, isVisible: true, isAttached: true);
		await ProductHeartIconButton.ClickAsync();
		await Helper.WaitBetweenActions(ProductHeartIconButton, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	/// <summary>
	/// Проверяет статус кнопки сердца в навбаре, ожидая её видимости.
	/// </summary>
	public async Task CheckHeartButtonStatus()
	{
		_productHeartInNavbar = Page.GetByLabel("Перейти в раздел отложенных товаров");
		await Helper.WaitBetweenActions(_productHeartInNavbar, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	/// <summary>
	/// Кликает на кнопку сердца в блоке рекомендаций "Книги из жанра", прокручивая и ожидая элементов.
	/// </summary>
	public async Task ClickProductRecommendationsHeartIconButton()
	{
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		var firstRecList = Page.Locator("section:has-text('Книги из жанра')");
		await Helper.WaitBetweenActions(firstRecList, isAttached: true, isVisible: true);
		await firstRecList.ScrollIntoViewIfNeededAsync();
		var listWithBooks = firstRecList.Locator("div.items-center").First;
		await listWithBooks.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(listWithBooks, isAttached: true, isVisible: true);
		_productListHeart = listWithBooks.Locator("button").Nth(1);
		await Helper.WaitBetweenActions(_productListHeart, isVisible: true, isAttached: true);
		await _productListHeart.ClickAsync();
		await Task.CompletedTask;
	}
	/// <summary>
	/// Проверяет статус кнопки сердца в рекомендациях, ожидая её видимости.
	/// </summary>
	public async Task CheckHeartButtonRecommendationsStatus()
	{
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await Helper.WaitBetweenActions(ProductHeartIconButton, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}

	/// <summary>
	/// Получает количество книг в избранном из навбара.
	/// </summary>
	/// <returns>Строка с количеством книг.</returns>
	public async Task<string> GetProductHeartInNavbar()
	{
		var heartInNavbar = _productHeartInNavbar.Locator("span").Nth(1);
		var heartInNavbarNumbers = await heartInNavbar.InnerTextAsync();
		return heartInNavbarNumbers;
	}
	/// <summary>
	/// Выполняет один клик на кнопку сердца и проверяет статус.
	/// </summary>
	public async Task OneClickProductHeartButton()
	{
		await ClickProductHeartIconButton();
		await CheckHeartButtonStatus();
	}

	/// <summary>
	/// Выполняет двойной клик на кнопку сердца (добавление и удаление).
	/// </summary>
	public async Task DoubleClickProductHeartButton()
	{
		await OneClickProductHeartButton();
		await OneClickProductHeartButton();
	}

	/// <summary>
	/// Выполняет один клик на кнопку сердца в рекомендациях и проверяет статус.
	/// </summary>
	public async Task OneClickProductHeartInRecRow()
	{
		await ClickProductRecommendationsHeartIconButton();
		await CheckHeartButtonRecommendationsStatus();
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
	}

	/// <summary>
	/// Выполняет двойной клик на кнопку сердца в рекомендациях (добавление и удаление).
	/// </summary>
	public async Task DoubleClickProductHeartInRecRow()
	{
		await OneClickProductHeartInRecRow();
		await OneClickProductHeartInRecRow();
	}
}