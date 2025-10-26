using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace labirinthAutoTesting.TestBase;

[TestFixture]
public abstract class BaseTest : PlaywrightTest
{
    protected IPage Page { get; private set; } = null!;
    protected IBrowserContext Context { get; private set; } = null!;

    [SetUp]
    public async Task GlobalSetup()
	{
		
        // Запускаем браузер
        var browser = await BrowserType.LaunchAsync(new()
		{
			Headless = false,
			SlowMo = 250,
			// Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
		});

        // Создаём контекст с куками
        Context = await browser.NewContextAsync();
		Console.WriteLine("Tracing started");
		// Start tracing for debugging
		try
		{
			await Context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true, Snapshots = true });
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error starting tracing: {ex.Message}");
		}

		// await Context.AddCookiesAsync(new[]
        // {
        //     new Cookie { Name = "id_post", Value = "1912", Domain = ".labirint.ru", Path = "./" },
        // });

  Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task GlobalTeardown()
	{
		Console.WriteLine("Attempting to stop tracing");
        try	
        {
			// Stop tracing and save to file
			await Context?.Tracing.StopAsync(new TracingStopOptions
			{
				Path = "../../../trace.zip"

			});
			 Console.WriteLine("Tracing stopped");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping tracing: {ex.Message}");
        }
		await Context.CloseAsync();
    }
	
    protected async Task GotoAsync(string path)
    {
        var baseUrl = "https://www.labirint.ru";
		await Page.GotoAsync(baseUrl.TrimEnd('/') + "/" + path.TrimStart('/'));
    }
}