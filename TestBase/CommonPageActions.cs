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
	public CommonPageActions(IPage page) : base(page)
	{
		_helper = new Helper(page);
	}

	/// <summary>
	/// Принимает модальное окно с политикой cookies, кликая на кнопку.
	/// </summary>
	public async Task AcceptModalWithCookies()
	{
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		var modalWithCookies = Page.Locator(".cookie-policy button");
		await modalWithCookies.ClickAsync();
		await Helper.WaitBetweenActions(modalWithCookies, isAttached: true);
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await Task.CompletedTask;
	}

	/// <summary>
	/// Находит первую кнопку сердца на странице и ожидает её видимости.
	/// </summary>
	public async Task GetFirstHeartOnPage()
	{

		_heartIcon = Page.Locator("a.icon-fave:has(span.header-sprite)").First;
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	/// <summary>
	/// Проверяет статус кнопки сердца, прокручивая её в область видимости.
	/// </summary>
	public async Task CheckHeartIconStatus()
	{
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await _heartIcon.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(HeartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}

	/// <summary>
	/// Находит кнопку сердца в навбаре и ожидает её видимости.
	/// </summary>
	public async Task GetHeartInNavbar()
	{
		_heartInNavbar = Page.Locator("#minwidth .top-link-main_putorder span.basket-in-dreambox-a");
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}
	/// <summary>
	/// Проверяет статус кнопки сердца в навбаре, прокручивая её в область видимости.
	/// </summary>
	public async Task CheckHeartInNavbarStatus()
	{
		await GetHeartInNavbar();
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await HeartInNavbar.ScrollIntoViewIfNeededAsync();
		await Helper.WaitBetweenActions(_heartInNavbar, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}


	/// <summary>
	/// Выполняет один клик на кнопку сердца.
	/// </summary>
	public async Task OneClikHeartIcon()
	{
		await _heartIcon.ClickAsync();
		await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await Task.CompletedTask;
	}
	/// <summary>
	/// Выполняет двойной клик на кнопку сердца (добавление и удаление).
	/// </summary>
	public async Task DoublClickHeartIcon()
	{
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await OneClikHeartIcon();
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await OneClikHeartIcon();
		await Helper.WaitBetweenActions(_heartIcon, isVisible: true, isAttached: true);
		await Task.CompletedTask;
	}

	/// <summary>
	/// Открывает тултип, выполняя двойной клик на кнопку сердца.
	/// </summary>
	public async Task OpenTooltip()
	{
		await GetFirstHeartOnPage();
		await CheckHeartIconStatus();
		await DoublClickHeartIcon();
	}

}