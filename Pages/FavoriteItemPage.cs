using labirinthAutoTesting.TestBase;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace labirinthAutoTesting.Pages;

public class FavoriteItemPage : BasePage
{
	private ILocator FavoritePage => Page.Locator(".book-title");

	public FavoriteItemPage(IPage page) : base(page)
	{
		// Ничего лишнего, просто инициализация базового класса
	}
	

}