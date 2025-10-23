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
		_listsWithBook = Page.Locator("product need-watch product_labeled watched gtm-watched");
		_firstBookInTheRow = _listsWithBook.First.Locator("div.products-row rows1").First;
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
		await _tooolTipWithActions.GetByText("Убрать из отложенных").ClickAsync();
	}
	
	public async Task MoveToFavList()
	{
		await AddBookToFavList();
		await _favoriteButton.ClickAsync();
		await _tooolTipWithActions.GetByText("Перейти к отложенным").ClickAsync();
	}
}