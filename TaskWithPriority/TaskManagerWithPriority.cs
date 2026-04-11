using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TaskManagerWithPriority
{
    public List<TaskWithPriorityy> Tasks { get; private set; }

    public TaskManagerWithPriority()
    {
        Tasks = new List<TaskWithPriorityy>();
        LoadTasks();
    }

    public void AddTask(TaskWithPriorityy task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }
        Tasks.Add(task);
        SaveTasks();
    }

    public void RemoveTask(TaskWithPriorityy task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }
        Tasks.Remove(task);
        SaveTasks();
    }

    public void ToggleTaskCompletion(TaskWithPriorityy task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }
        task.IsCompleted = !task.IsCompleted;
        SaveTasks();
    }

    public List<TaskWithPriorityy> SortTasksByPriority()
    {
        return Tasks.OrderByDescending(t => t.Priority).ToList();
    }

    private void SaveTasks()
    {
        File.WriteAllLines("tasks.txt", Tasks.Select(t => $"{t.Description}|{(int)t.Priority}|{t.IsCompleted}|{t.Deadline.ToString("yyyy-MM-dd HH: mm:ss")}")); 
    } 


    private void LoadTasks()
    {
        if (File.Exists("tasks.txt"))
        {
            var lines = File.ReadAllLines("tasks.txt");
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 4)
                {
                    int priority;
                    bool isCompleted;
                    DateTime deadline;
                    if (int.TryParse(parts[1], out priority) && bool.TryParse(parts[2], out
isCompleted) && DateTime.TryParse(parts[3], out deadline))
                    {
                        TaskWithPriorityy task = new TaskWithPriorityy(parts[0], (Priority)priority,
deadline);
                        task.IsCompleted = isCompleted;
                        Tasks.Add(task);
                    }
                }
            }
        }
    }
}