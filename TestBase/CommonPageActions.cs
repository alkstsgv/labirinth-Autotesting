using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using labirinthAutoTesting.Pages;
using labirinthAutoTesting.Helpers;
namespace labirinthAutoTesting.TestBase;

public class CommonPageActions : BasePage
{
	
	private PlaywrightTest _playwright = new PlaywrightTest();
	private ILocator _heartIcon = null!;
	public ILocator HeartIcon => _heartIcon;
	private ILocator _heartInNavbar = null!;
	public ILocator HeartInNavbar => _heartInNavbar;
	private Helper _helper; public Helper Helper => _helper ??= new Helper(Page);
	public CommonPageActions (IPage page) : base(page)
	{
		_helper = new Helper(page);
	}

	public async Task AcceptModalWithCookies()
	{
		var modalWithCookies = Page.Locator(".cookie-policy button");
		await modalWithCookies.ClickAsync();
		await Helper.WaitBetweenActions(modalWithCookies, isAttached: true);
		await Task.CompletedTask;
	}

	public async Task GetFirstHeartOnPage()
	{
		_heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	public async Task CheckHeartIconStatus()
	{
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await _heartIcon.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}

	public async Task GetHeartInNavbar()
	{
		_heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	public async Task CheckHeartInNavbarStatus()
	{
		await GetHeartInNavbar();
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await HeartInNavbar.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}



	public async Task DoubleHeartIconClick()
	{
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await _heartIcon.ClickAsync();
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await _heartIcon.ClickAsync();
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}

}