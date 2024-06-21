using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mauiprojekt {
    public class TaskDatabase {
        readonly SQLiteAsyncConnection _database;

        public TaskDatabase(string dbPath) {
            try {
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<TaskModel>().Wait();
            } catch (Exception ex) {
                // Log or handle the exception as needed
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        public Task<List<TaskModel>> GetTasksAsync() {
            return _database.Table<TaskModel>().ToListAsync();
        }


        public Task<int> SaveTaskAsync(TaskModel task) {
            if (task.Id != -1) {
                return _database.UpdateAsync(task);
            } else {
                return _database.InsertAsync(task);
            }
        }

        public Task<int> DeleteTaskAsync(TaskModel task) {
            return _database.DeleteAsync(task);
        }
    }
}
