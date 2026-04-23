using System;

public enum Priority
{
    Низкий,
    Средний,
    Высокий
}

public class TaskWithPriorityy
{
    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(Description), "Описание не может быть null");
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Описание не может быть пустым", nameof(Description));
            _description = value;
        }
    }
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime Deadline { get; set; }

    public TaskWithPriorityy(string description, Priority priority, DateTime deadline)
    {
        if (description == null)
            throw new ArgumentNullException(nameof(description), "Описание не может быть null");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Описание не может быть пустым", nameof(description));

        _description = description;
        Priority = priority;
        IsCompleted = false;
        Deadline = deadline;
    }
}