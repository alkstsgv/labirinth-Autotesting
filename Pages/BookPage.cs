using labirinthAutoTesting.TestBase;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace labirinthAutoTesting.Pages;

public class BookPage : BasePage
{
	private ILocator _productHeartIconButton = null!;
	public ILocator ProductHeartIconButton => _productHeartIconButton ??= Page.Locator(".items-center.cursor-pointer button");
	private ILocator _productHeartInNavbar = null!;
	public ILocator ProductHeartInNavbar => _productHeartInNavbar ??= Page.Locator("Перейти в раздел отложенных товаров");
	private ILocator _productCard = null!;
	public ILocator ProductCard => _productCard ??= Page.Locator("a.cover:has(span.default-cover)");
	public BookPage(IPage page) : base(page)
	{
		// Ничего лишнего, просто инициализация базового класса
	}

	public async Task ClickFirstBookOnPage()
	{
		await ProductCard.First.ClickAsync();
	}
	public async Task ClickProductHeartIconButton()
	{
		await ProductHeartIconButton.ClickAsync();
	}
}