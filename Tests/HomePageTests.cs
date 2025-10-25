using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;

using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class HomePageTester : BaseTest
{
	private HomePage _homePage = null!;
	// private BaseTest _base = null!;

	[SetUp]
    public void SetupPageObjects()
    {
        _homePage = new HomePage(Page);
    }

	[Test]
	public async Task CheckAddBookToFavList()
	{
		await GotoAsync("/");
		// await _homePage.ClickLocator(".cookie-policy button");
		await _homePage.AcceptModalWithCookies();
		await _homePage.AddBookToFavList();
	}
	
}