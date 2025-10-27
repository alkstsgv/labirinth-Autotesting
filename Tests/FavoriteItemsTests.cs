using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.VisualBasic;

using labirinthAutoTesting.Pages;
using labirinthAutoTesting.TestBase;

namespace labirinthAutoTesting.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class FavoriteItemsTests : BaseTest
{
	private FavoriteItemPage _favoriteItemPage = null!;
	public FavoriteItemPage FavoriteItemPage => _favoriteItemPage;
	private CommonPageActions _commonPageActions = null!;
	public CommonPageActions CommonPageActions => _commonPageActions;

	[SetUp]
	public void SetupPageObjects()
	{
		_favoriteItemPage = new FavoriteItemPage(Page);
		_commonPageActions = new CommonPageActions(Page);
	}
	
}
