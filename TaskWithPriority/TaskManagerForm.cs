using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskWithPriority
{
    public partial class TaskManagerForm : Form
    {
        private TaskManagerWithPriority taskManager;
        private TextBox descriptionTextBox;
        private ComboBox priorityComboBox;
        private DateTimePicker deadlinePicker;
        private Button addTaskButton;
        private Button removeTaskButton;
        private Button toggleCompletionButton;
        private ComboBox sortByPriorityComboBox;
        private Button sortByPriorityButton;
        private ListBox tasksListBox;

        public TaskManagerForm()
        {
            this.Text = "Управление задачами с приоритетом";
            this.Width = 600;
            this.Height = 500;

            descriptionTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200,
            };

            priorityComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(220, 10),
                Width = 100,
                Items = { "Низкий", "Средний", "Высокий" }
            };

            deadlinePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(330, 10)
            };

            addTaskButton = new Button
            {
                Location = new System.Drawing.Point(10, 40),
                Text = "Добавить",
                Width = 100
            };
            addTaskButton.Click += AddTaskButton_Click;

            removeTaskButton = new Button
            {
                Location = new System.Drawing.Point(120, 40),
                Text = "Удалить",
                Width = 100
            };
            removeTaskButton.Click += RemoveTaskButton_Click;

            toggleCompletionButton = new Button
            {
                Location = new System.Drawing.Point(220, 40),
                Text = "Отметить",
                Width = 100
            };
            toggleCompletionButton.Click += ToggleCompletionButton_Click;

            sortByPriorityComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(10, 70),
                Width = 100,
                Items = { "По приоритету" }
            };

            sortByPriorityButton = new Button
            {
                Location = new System.Drawing.Point(120, 70),
                Text = "Сортировать",
                Width = 100
            };
            sortByPriorityButton.Click += SortByPriorityButton_Click;

            tasksListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 100),
                Width = 560,
                Height = 300
            };

            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(priorityComboBox);
            this.Controls.Add(deadlinePicker);
            this.Controls.Add(addTaskButton);
            this.Controls.Add(removeTaskButton);
            this.Controls.Add(toggleCompletionButton);
            this.Controls.Add(sortByPriorityComboBox);
            this.Controls.Add(sortByPriorityButton);
            this.Controls.Add(tasksListBox);

            taskManager = new TaskManagerWithPriority();
            UpdateTasksList();
        }

        private void UpdateTasksList()
        {
            tasksListBox.Items.Clear();
            foreach (var task in taskManager.Tasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ )";
                tasksListBox.Items.Add($"{status} {task.Description} (Приоритет: {task.Priority})");
            }
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                MessageBox.Show("Введите описание задачи!");
                return;
            }
            Priority priority = (Priority)Enum.Parse(typeof(Priority),
    priorityComboBox.SelectedItem.ToString());
            DateTime deadline = deadlinePicker.Value;
            TaskWithPriorityy newTask = new TaskWithPriorityy(descriptionTextBox.Text, priority,
    deadline);
            try
            {
                taskManager.AddTask(newTask);
                descriptionTextBox.Clear();
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveTaskButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для удаления!");
                return;
            }
            string selectedItem = tasksListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string description = parts[2];
                var taskToRemove = taskManager.Tasks.Find(t => t.Description == description);
                if (taskToRemove != null)
                {
                    try
                    {
                        taskManager.RemoveTask(taskToRemove);
                        UpdateTasksList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ToggleCompletionButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для изменения статуса!");
                return;
            }
            string selectedItem = tasksListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string description = parts[2];
                var taskToToggle = taskManager.Tasks.Find(t => t.Description == description);
                if (taskToToggle != null)
                {
                    try
                    {
                        taskManager.ToggleTaskCompletion(taskToToggle);
                        UpdateTasksList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void SortByPriorityButton_Click(object sender, EventArgs e)
        {
            var sortedTasks = taskManager.SortTasksByPriority();
            tasksListBox.Items.Clear();
            foreach (var task in sortedTasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ )";
                tasksListBox.Items.Add($"{status} {task.Description} (Приоритет: {task.Priority})");
            }
        }
    }
}
