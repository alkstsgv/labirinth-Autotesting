using labirinthAutoTesting.TestBase;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace labirinthAutoTesting.Pages;

public class BookPage : BasePage
{
	private ILocator BookTitle => Page.Locator(".book-title");

	public BookPage(IPage page) : base(page)
	{
		// Ничего лишнего, просто инициализация базового класса
	}
	
	public async Task<string> GetBookTitleAsync()
    {
        return await BookTitle.InnerTextAsync();
    }
}