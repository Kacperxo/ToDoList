namespace ToDoList.Models
{
    public class TaskModel : ObservableObject
    {
        #region variables
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsNotificationSent { get; set; }
        #endregion

        #region constructors
        public TaskModel() {}
        public TaskModel(string title, DateTime dueDate)
        {
            Title = title;
            Description = string.Empty;
            DueDate = dueDate;
            IsCompleted = false;
        }
        public TaskModel(string title, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            IsCompleted = false;
            IsNotificationSent = false;
        }
        #endregion
    }
}
