using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiprojekt {
    [AddINotifyPropertyChangedInterface]
    public class TaskModel {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Boolean IsImportant { get; set; }
        public Boolean IsCompleted { get; set; }

        //   public string Attachment { get; set; }
    }
}
