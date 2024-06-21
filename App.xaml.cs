using Microsoft.Extensions.DependencyInjection;

namespace mauiprojekt;

public partial class App : Application {
    public App(IServiceProvider serviceProvider) {
        InitializeComponent();

        var database = serviceProvider.GetRequiredService<TaskDatabase>();

        var taskListViewModel = serviceProvider.GetRequiredService<TaskListViewModel>();

        var taskListView = new TaskListView(taskListViewModel, serviceProvider, database);

        MainPage = new NavigationPage(taskListView);
    }
}
