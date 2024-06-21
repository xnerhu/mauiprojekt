using System;
using Microsoft.Maui.Controls;

namespace mauiprojekt {
    partial class TaskListView : ContentPage {
        private readonly IServiceProvider _serviceProvider;
        private readonly TaskDatabase taskDatabase; // Add this line

        public TaskListView(TaskListViewModel viewModel, IServiceProvider serviceProvider, TaskDatabase taskDatabase) {
            InitializeComponent();
            this.BindingContext = viewModel;
            _serviceProvider = serviceProvider;
            this.taskDatabase = taskDatabase;

        }

        private async Task gotoTaskDetailsView(TaskModel task) {
            if (task == null)
                return;
            var taskDetailsViewModel = ActivatorUtilities.CreateInstance<TaskDetailsViewModel>(_serviceProvider, task, taskDatabase, this.BindingContext as TaskListViewModel);
            var taskDetailsView = ActivatorUtilities.CreateInstance<TaskDetailsView>(_serviceProvider, taskDetailsViewModel);
            await Navigation.PushAsync(taskDetailsView);
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            var t = ((TaskListViewModel)this.BindingContext);
            if (t != null) {
                await t.LoadTasks();
            }
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e) {
            if (e.Item != null) {
                var selectedTask = e.Item as TaskModel;
                if (selectedTask != null) {
                    //await gotoTaskDetailsView(selectedTask);
                    var viewModel = (TaskListViewModel)this.BindingContext;
                    viewModel.EditTaskCommand.Execute(selectedTask);
                }

                // Clear the selection
                ((ListView)sender).SelectedItem = null;
            }
        }

        private async void OnFabClicked(object sender, EventArgs e) {
            var view = ((TaskListViewModel)this.BindingContext);
            view.debugText = DateTime.Now.ToString() + "|" + view.Tasks.Count;
            TaskModel task = new TaskModel {
                Id = -1,
                Description = "",
                Title = "",
                CreatedAt = DateTime.Now,
                IsImportant = false,
                IsCompleted = false,
                // Image = "",
            };
            await gotoTaskDetailsView(task);
        }
    }
}
