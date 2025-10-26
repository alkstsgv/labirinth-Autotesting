using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using labirinthAutoTesting.Pages;

namespace labirinthAutoTesting.TestBase;

public class CommonPageActions : BasePage
{
	
	private PlaywrightTest _playwright = new PlaywrightTest();
	private ILocator _heartIcon = null!;
	public ILocator HeartIcon => _heartIcon;
	private ILocator _heartInNavbar = null!;
	public ILocator HeartInNavbar => _heartInNavbar;
	public CommonPageActions (IPage page) : base(page)
	{
		
	}

	public async Task AcceptModalWithCookies()
	{
		var modalWithCookies = ".cookie-policy button";
		await ClickLocator(modalWithCookies);
	}

	public async Task GetFirstHeartOnPage()
	{
		_heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await Task.CompletedTask;
		
	}
	public async Task CheckHeartIconStatus()
	{
		await _heartIcon.IsVisibleAsync();
		await _heartIcon.IsEnabledAsync();
		await _heartIcon.ScrollIntoViewIfNeededAsync();
		await Task.CompletedTask;
	}

	public async Task GetHeartInNavbar()
	{
		_heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		await Task.CompletedTask;
	}
	public async Task CheckHeartInNavbarStatus()
	{
		await _heartInNavbar.IsVisibleAsync();
		await _heartInNavbar.IsEnabledAsync();
		await _heartInNavbar.ScrollIntoViewIfNeededAsync();
		await Task.CompletedTask;
	}

	public async Task ClickFirstBookOnPage()
	{
		await GetFirstHeartOnPage();
		await CheckHeartIconStatus();
		var productCard = _heartIcon.Locator("xpath=ancestor::div[contains(@class, 'product')]").First;
		await productCard.ClickAsync();
		await Task.CompletedTask;

	}

	public async Task DoubleHeartIconClick()
	{
		await _heartIcon.ClickAsync();
		await _heartIcon.ClickAsync();
		await Task.CompletedTask;
	}
}