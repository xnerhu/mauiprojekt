using OpenAI_API;
using System.Windows.Input;

namespace mauiprojekt;

public partial class TaskDetailsView : ContentPage {


    public TaskDetailsView(TaskDetailsViewModel viewModel) {
        InitializeComponent();
        this.BindingContext = viewModel;

    }

    private async void OnSaveClicked(object sender, EventArgs e) {
        var viewModel = this.BindingContext as TaskDetailsViewModel;
        var task = viewModel.Task;

        var taskListViewModel = viewModel.TaskListViewModel as TaskListViewModel;

        Console.WriteLine("SAVING");
        await viewModel.TaskDatabase.SaveTaskAsync(task);
        Console.WriteLine("LOADING NEW");
        await taskListViewModel.LoadTasks();

        await Navigation.PopAsync();
    }

    private async void OnRefineClicked(object sender, EventArgs e) {
        var viewModel = this.BindingContext as TaskDetailsViewModel;
        var task = viewModel.Task;
        viewModel.RefineDescription();
    }
}