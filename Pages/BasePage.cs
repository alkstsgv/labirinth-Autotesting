using Microsoft.Playwright;

namespace labirinthAutoTesting.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

	public async Task WaitForPageToLoadAsync()
	{
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
	}
	
	public async Task ClickLocator(string selector)
	{
		var locator = Page.Locator(selector);
		await locator.ClickAsync();
	}
}