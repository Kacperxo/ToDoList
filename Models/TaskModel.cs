namespace ToDoList.Models
{
    public class TaskModel
    {
        #region variables
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        #endregion

        #region constructors
        public TaskModel() { }
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
        }
        #endregion

        #region methods
        public void UpdateTitle(string newTitle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newTitle))
                {
                    throw new ArgumentException("Title cannot be empty or whitespace.");
                }
                if (newTitle.Length > 50)
                {
                    throw new ArgumentException("Title cannot exceed 50 characters.");
                }
                Title = newTitle;
                UpdatedAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating title: {ex.Message}");
            }
        }
        public void UpdateDescription(string newDescription)
        {
            try
            {
                if (newDescription.Length > 500)
                {
                    throw new ArgumentException("Description cannot exceed 500 characters.");
                }
                Description = newDescription;
                UpdatedAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating description: {ex.Message}");
            }
        }
        public void UpdateDueDate(DateTime newDueDate)
        {
            try
            {
                if (newDueDate < DateTime.Now)
                {
                    throw new ArgumentException("Due date cannot be in the past.");
                }
                DueDate = newDueDate;
                UpdatedAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating due date: {ex.Message}");
            }
        }
        public void MarkAsCompleted()
        {
            IsCompleted = true;
            UpdatedAt = DateTime.Now;
        }
        public void MarkAsIncomplete()
        {
            IsCompleted = false;
            UpdatedAt = DateTime.Now;
        }
        public void UpdateTable(string title, string description, DateTime dueDate, bool isCompleted)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException("Title cannot be empty or whitespace.");
                }
                if (title.Length > 50)
                {
                    throw new ArgumentException("Title cannot exceed 50 characters.");
                }
                if (description.Length > 500)
                {
                    throw new ArgumentException("Description cannot exceed 500 characters.");
                }
                if (dueDate < DateTime.Now)
                {
                    throw new ArgumentException("Due date cannot be in the past.");
                }
                UpdateTitle(title);
                UpdateDescription(description);
                UpdateDueDate(dueDate);
                IsCompleted = isCompleted;
                UpdatedAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating task: {ex.Message}");
            }
        }
        public override string ToString()
        {
            return $"{Title} - Due: {DueDate.ToShortDateString()} - Completed: {IsCompleted}";
        }
        #endregion
    }
}
