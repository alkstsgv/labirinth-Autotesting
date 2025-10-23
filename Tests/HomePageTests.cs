using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using labirinthAutoTesting.Pages;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class HomePageTester : PlaywrightTest
{
    private IPage _page = null!;
    private HomePage _homePage = null!;

    [SetUp]
    public async Task Setup()
    {
        // Запускаем браузер
        var browser = await BrowserType.LaunchAsync(new()
		{
			Headless = false,
			SlowMo = 500,
			// Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" } // важно для Linux/Docker
			 Args = new[] { "--start-maximized" }
		});

        var context = await browser.NewContextAsync();

        // Устанавливаем куки, чтобы избежать регионального попапа
        await context.AddCookiesAsync(new[]
        {
            new Cookie { Name = "region", Value = "77", Domain = ".labirint.ru", Path = "/" },
            new Cookie { Name = "cookie_accepted", Value = "1", Domain = ".labirint.ru", Path = "/" }
        });

        _page = await context.NewPageAsync();
        _homePage = new HomePage(_page);
    }

    [Test]
    public async Task CheckAddBookToFavList()
    {
        await _page.GotoAsync("https://www.labirint.ru");

        // Дождёмся основного контента
        await _page.WaitForSelectorAsync("body", new() { State = WaitForSelectorState.Visible });

        await _homePage.AddBookToFavList();

        // Проверим, что заголовок — про Лабиринт, а не "Playwright"
        await Expect(_page).ToHaveTitleAsync(new Regex(@".*Лабиринт.*", RegexOptions.IgnoreCase));
    }

    [TearDown]
    public async Task TearDown()
    {
        await _page?.CloseAsync();
        await _page?.Context?.Browser?.CloseAsync();
    }
}