using System.Linq;
using DotNetEnv;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace labirinthAutoTesting.TestBase;

[TestFixture]
public abstract class BaseTest : PlaywrightTest
{
	protected IPage Page { get; private set; } = null!;
	protected IBrowserContext Context { get; private set; } = null!;

	/// <summary>
	/// Глобальная настройка теста: загрузка переменных окружения и запуск браузера.
	/// </summary>
	[SetUp]
	public async Task GlobalSetup()
	{
		var envFile = File.Exists(".env") ? ".env" : ".env_orig";
		string? headlessStr = null;
		string? slowMoStr = null;

		if (File.Exists(envFile))
		{
			var env = Env.Load(envFile).ToDictionary(e => e.Key, e => e.Value);
			headlessStr = env.GetValueOrDefault("HEADLESS");
			slowMoStr = env.GetValueOrDefault("SLOWMO");

		}

		bool headless = (headlessStr ?? "true").ToLower() == "true";
		float slowMo = float.TryParse(slowMoStr, out var sm) ? sm : 0;

		Console.WriteLine($"EnvFile: {envFile}, Headless mode: {headless}, SlowMo: {slowMo}");

		var browser = await BrowserType.LaunchAsync(new()
		{
			Headless = headless,
			SlowMo = slowMo,
			Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
		});


		// Создаём контекст с куками
		Context = await browser.NewContextAsync();
		Console.WriteLine("Tracing started");
		try
		{
			await Context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true, Snapshots = true });
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error starting tracing: {ex.Message}");
		}
		Page = await Context.NewPageAsync();
	}

	/// <summary>
	/// Глобальная очистка теста: остановка трассировки и закрытие контекста.
	/// </summary>
	[TearDown]
	public async Task GlobalTeardown()
	{
		Console.WriteLine("Attempting to stop tracing");
		try
		{
			if (Context != null)
			{
				/// Остановка записи теста. Запись в trace.zip
				await Context?.Tracing.StopAsync(new TracingStopOptions
				{
					Path = "../../../trace.zip"

				});

				Console.WriteLine("Tracing stopped");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error stopping tracing: {ex.Message}");
		}
		await Context.CloseAsync();
	}

	/// <summary>
	/// Переходит на страницу сайта labirint.ru с указанным путём.
	/// </summary>
	/// <param name="path">Путь к странице (например, "/").</param>
	protected async Task GotoAsync(string path)
	{
		var baseUrl = "https://www.labirint.ru" ?? string.Empty;
		await Page.GotoAsync(baseUrl.TrimEnd('/') + "/" + path.TrimStart('/'));
	}
}
