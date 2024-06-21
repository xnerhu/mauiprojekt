using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mauiprojekt {
    [AddINotifyPropertyChangedInterface]
    public class TaskListViewModel {
        public List<TaskModel> Tasks { get; set; }
        public String debugText { get; set; }
        public TaskDatabase _database;
        public ICommand EditTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }

        public ICommand CompleteTaskCommand { get; }

        public TaskListViewModel() {
            Tasks = new List<TaskModel>();
        }

        public TaskListViewModel(TaskDatabase _database) {
            debugText = "xd";
            Tasks = new List<TaskModel>();
            this._database = _database;
            EditTaskCommand = new Command<TaskModel>(EditTask);
            RemoveTaskCommand = new Command<TaskModel>(RemoveTask);
            CompleteTaskCommand = new Command<TaskModel>(CompleteTask);
        }

        private async void EditTask(TaskModel task) {
            if (task == null)
                return;
            var taskDetailsViewModel = new TaskDetailsViewModel(task, _database, this);
            var taskDetailsView = new TaskDetailsView(taskDetailsViewModel);
            await Application.Current.MainPage.Navigation.PushAsync(taskDetailsView);
        }

        private async void RemoveTask(TaskModel task) {
            Console.WriteLine("Removing task", task);
            if (task == null)
                return;
            await _database.DeleteTaskAsync(task);
            Tasks = Tasks.Where(t => t.Id != task.Id).ToList();
        }

        private async void CompleteTask(TaskModel task) {
            if (task == null)
                return;
            task.IsCompleted = task.IsCompleted == null ? true : !task.IsCompleted;
            await _database.SaveTaskAsync(task);
            Tasks = Tasks;
        }


        public async Task LoadTasks() {
            var _tasks = await _database.GetTasksAsync();
            _tasks = _tasks.OrderByDescending(task => task.CreatedAt).ToList();
            Tasks.Clear();
            Console.WriteLine("LOADED TASKS" + _tasks.Count);
            this.Tasks = _tasks;
            this.debugText = "loaded trasks" + _tasks.Count;
            /* this.Tasks = new List<TaskModel> {
                  new TaskModel {
                      Title = "Task 1",
                      Description = "Description 1",
                      CreatedAt = DateTime.Now
                  },
                  new TaskModel {
                      Title = "Task 2",
                      Description = "Description 2",
                      CreatedAt = DateTime.Now
                  },
                  new TaskModel {
                      Title = "Task 3",
                      Description = "Description 3",
                      CreatedAt = DateTime.Now
                  }
              };*/

        }
        /* public TaskListViewModel() {
             this.Tasks = new List<TaskModel> {
                 new TaskModel {
                     Title = "Task 1",
                     Description = "Description 1",
                     CreatedAt = DateTime.Now
                 },
                 new TaskModel {
                     Title = "Task 2",
                     Description = "Description 2",
                     CreatedAt = DateTime.Now
                 },
                 new TaskModel {
                     Title = "Task 3",
                     Description = "Description 3",
                     CreatedAt = DateTime.Now
                 }
             };
         }*/
    }
}