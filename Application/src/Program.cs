
using Microsoft.Extensions.DependencyInjection;

Service.Register(new ServiceCollection());
AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .StartWithClassicDesktopLifetime(args);

