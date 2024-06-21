namespace mauiprojekt;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasks.db3");

        var db = new TaskDatabase(dbPath);
        builder.Services.AddSingleton(db);

        // Register TaskListViewModel and TaskListView
        builder.Services.AddSingleton<TaskListView>();

        // Register TaskDetailsViewModel and TaskDetailsView
        builder.Services.AddTransient<TaskDetailsViewModel>();
        builder.Services.AddTransient<TaskDetailsView>();
        builder.Services.AddSingleton(new TaskListViewModel(db));
        // builder.Services.AddSingleton<TaskListViewModel>();

        return builder.Build();
    }
}