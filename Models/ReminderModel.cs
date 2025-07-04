using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ReminderModel : ObservableObject
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ReminderDateTime { get; set; }
        public int TaskId { get; set; }
        public virtual TaskModel Task { get; set; } = null!;
        
        public ReminderModel(){}
    }
}
