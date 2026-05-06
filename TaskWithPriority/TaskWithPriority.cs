using System;

public enum Priority
{
    Низкий,
    Средний,
    Высокий
}

public enum NotificationTime
{
    ForNo = 0,
    For10min = 10,
    For60min = 60,
    For24h = 1440
}

public class TaskWithPriorityy
{
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime Deadline { get; set; }
    public NotificationTime NotificationTime { get; set; }
    public bool NotificationShown { get; set; }

    public TaskWithPriorityy(string description, Priority priority, DateTime deadline)
    {
        Description = description;
        Priority = priority;
        IsCompleted = false;
        Deadline = deadline;
        NotificationTime = NotificationTime.ForNo;
        NotificationShown = false;
    }

    public bool ShouldShowNotification()
    {
        if (IsCompleted || NotificationShown || NotificationTime == NotificationTime.ForNo)
            return false;

        TimeSpan timeUntilDeadline = Deadline - DateTime.Now;
        int minutesUntilDeadline = (int)timeUntilDeadline.TotalMinutes;

        return minutesUntilDeadline <= (int)NotificationTime && minutesUntilDeadline > 0;
    }
}