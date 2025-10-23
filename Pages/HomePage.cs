using Microsoft.Playwright;

namespace LabirintTests.Pages;

public class HomePage : BasePage
{
    private readonly ILocator _delayedLink;
    private readonly ILocator _firstBookDelayButton;

    public HomePage(IPage page) : base(page)
    {
        _delayedLink = Page.GetByRole(AriaRole.Link, new() { Name = "Отложенные товары" });
        _firstBookDelayButton = Page.Locator("div.product-card").First.Locator("button:has-text('Отложить')");
    }

    public async Task GoToDelayedItemsAsync()
    {
        await _delayedLink.ClickAsync();
        await WaitForPageToLoadAsync();
    }

    public async Task AddFirstBookToDelayedAsync()
    {
        await _firstBookDelayButton.ClickAsync();
        // Ждём появления уведомления или изменения состояния
        await Page.WaitForSelectorAsync("text=Товар отложен");
    }
}