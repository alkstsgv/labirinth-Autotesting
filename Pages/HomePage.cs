using Microsoft.Playwright;

namespace labirinthAutoTesting.Pages;
public class HomePage : BasePage
{
	private readonly ILocator _favoriteButton;
	private readonly ILocator _listsWithBook;
	private readonly ILocator _firstBookInTheRow;
	private readonly ILocator _tooolTipWithActions;

    public HomePage(IPage page) : base(page)
	{
		_listsWithBook = Page.Locator(".main-block-carousel .bestsellers .main-carousel");
		_firstBookInTheRow = _listsWithBook.First.Locator(".product-padding").First;
		_favoriteButton = _firstBookInTheRow.Locator("header-sprite");
		_tooolTipWithActions = Page.Locator("js-putorder-block-change b-dropdown-window");
    }


	public async Task AddBookToFavList()
	{
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