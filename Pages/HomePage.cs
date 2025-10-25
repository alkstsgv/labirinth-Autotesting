using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace labirinthAutoTesting.Pages;
public class HomePage : BasePage
{
	public  ILocator _favoriteButton = null!;
	public  ILocator _listsWithBook = null!;
	public  ILocator _firstBookInTheRow = null!;
	public  ILocator _tooolTipWithActions = null!;
	public PlaywrightTest _playwright = new PlaywrightTest();

	public HomePage(IPage page) : base(page)
	{
		_listsWithBook = Page.Locator("#right-inner .main-block-carousel.bestsellers").First;
		_firstBookInTheRow = _listsWithBook.Locator(".products-row.rows1").First;
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
		await _playwright.Expect(_listsWithBook).ToBeInViewportAsync();
		await _playwright.Expect(_firstBookInTheRow).ToBeInViewportAsync();
		await _favoriteButton.ClickAsync();
	}

	public async Task DeleteFromFavList()
	{
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