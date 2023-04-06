using TicTacToeClient.Views;

namespace TicTacToeClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<EnterNameViewModel>();

        builder.Services.AddSingleton<EnterNamePage>();

        builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<HighScoresViewModel>();

		builder.Services.AddSingleton<HighScoresPage>();

		return builder.Build();
	}
}
