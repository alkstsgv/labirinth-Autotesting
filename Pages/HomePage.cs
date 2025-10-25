using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace labirinthAutoTesting.Pages;
public class HomePage : BasePage
{
	private readonly ILocator _favoriteButton = null!;
	private readonly ILocator _listsWithBook = null!;
	private readonly ILocator _firstBookInTheRow = null!;
	private readonly ILocator _tooolTipWithActions = null!;
	private readonly PlaywrightTest _test = new PlaywrightTest();

	public HomePage(IPage page) : base(page)
	{
		_listsWithBook = Page.Locator("#right-inner .main-block-carousel.bestsellers").First;
		_firstBookInTheRow = _listsWithBook.Locator(".products-row.rows1");
		_favoriteButton = _firstBookInTheRow.First.Locator(".icon-fave").First;
		_tooolTipWithActions = Page.Locator("js-putorder-block-change b-dropdown-window");
	}

	public async Task AcceptModalWithCookies()
	{
		var modalWithCookies = ".cookie-policy button";
		await ClickLocator(modalWithCookies);
	}
	
	public async Task AddBookToFavList()
	{
		await _listsWithBook.IsVisibleAsync();
		await _listsWithBook.ScrollIntoViewIfNeededAsync();
		await _test.Expect(_listsWithBook).ToBeInViewportAsync();
		await _test.Expect(_firstBookInTheRow).ToBeInViewportAsync();
		await _favoriteButton.ClickAsync();
	}

	public async Task DeleteFromFavList()
	{
		await AddBookToFavList();
		await _favoriteButton.ClickAsync();
		var removeButton = _tooolTipWithActions.GetByText("Убрать из отложенных");
		await removeButton.ClickAsync();
	}
	
	public async Task MoveToFavList()
	{
		await AddBookToFavList();
		await _favoriteButton.ClickAsync();
		var moveButton = _tooolTipWithActions.GetByText("Перейти к отложенным");
		await moveButton.ClickAsync();
	}
}