Service.Register();
AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI()
            .WithDeveloperTools()
            .StartWithClassicDesktopLifetime(args);

