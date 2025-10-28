using CommunityToolkit.Maui;
using JournalApp.ExtensionClasses;
using Microsoft.Extensions.Logging;

namespace JournalApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Segoe-Fluent-Icons.ttf", "SegoeFluentIcons");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddServicesToDIContainer();

        return builder.Build();
	}
}
