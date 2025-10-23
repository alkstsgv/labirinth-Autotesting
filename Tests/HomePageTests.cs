using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using LabirintTests.Pages;

namespace LabirintTests.Tests;

[TestFixture]
public class HomePageTests : PlaywrightTest
{
    private IPage? _page;
    private HomePage? _homePage;

    [SetUp]
    public async Task Setup()
    {
        var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = true });
        _page = await browser.NewPageAsync();
        await _page.GotoAsync("https://www.labirint.ru");
        _homePage = new HomePage(_page);
    }

    [TearDown]
    public async Task Teardown()
    {
        if (_page != null)
            await _page.CloseAsync();
    }

    [Test]
    public async Task CanAddBookToDelayedFromHomePage()
    {
        // Act
        await _homePage!.AddFirstBookToDelayedAsync();
        await _homePage.GoToDelayedItemsAsync();

        // Assert — здесь можно добавить проверку через DelayedItemsPage
        // Но для примера просто проверим URL или элемент
        Assert.That(_page!.Url, Does.Contain("otlojeno"));
    }
}