using labirinthAutoTesting.TestBase;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using labirinthAutoTesting.Pages;
namespace labirinthAutoTesting.Helpers;

public class Helper : BasePage
{
	public Helper(IPage page) : base(page)
	{

	}

	public async Task WaitBetweenActions(ILocator locator, bool isVisible = false, bool isAttached = false, bool isDetached = false, bool isHidden = false)
	{
		if (isAttached == true)
		{
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await locator.WaitForAsync(new() { State = WaitForSelectorState.Attached });
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		}
		if (isVisible == true)
		{
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible });
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		}
		if (isDetached == true)
		{
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await locator.WaitForAsync(new() { State = WaitForSelectorState.Detached });
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		}
		if (isHidden == true)
		{
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await locator.WaitForAsync(new() { State = WaitForSelectorState.Hidden });
			await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		}
	}

}