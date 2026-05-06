using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TaskWithPriority
{
    public partial class TaskManagerForm : Form
    {
        private TaskManagerWithPriority taskManager;
        private Timer notificationTimer;

        // Заголовки
        private Label descriptionLabel;
        private Label priorityLabel;
        private Label deadlineLabel;
        private Label notificationLabel;
        private Label sortLabel;
        private Label tasksLabel;

        // Поля ввода
        private TextBox descriptionTextBox;
        private ComboBox priorityComboBox;
        private DateTimePicker deadlinePicker;
        private ComboBox notificationComboBox;

        // Кнопки
        private Button addTaskButton;
        private Button removeTaskButton;
        private Button toggleCompletionButton;
        private Button setNotificationButton;
        private ComboBox sortByPriorityComboBox;
        private Button sortByPriorityButton;

        // Список задач
        private ListBox tasksListBox;

        public TaskManagerForm()
        {
            this.Text = "Управление задачами с приоритетом";
            this.Width = 750;
            this.Height = 650;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            InitializeControls();
            InitializeNotificationTimer();

            taskManager = new TaskManagerWithPriority();
            UpdateTasksList();
        }

        private void InitializeControls()
        {
            // ========== ЗАГОЛОВКИ ==========

            descriptionLabel = new Label
            {
                Location = new Point(20, 20),
                Width = 200,
                Height = 20,
                Text = "Описание задачи:"
            };

            priorityLabel = new Label
            {
                Location = new Point(240, 20),
                Width = 120,
                Height = 20,
                Text = "Приоритет:"
            };

            deadlineLabel = new Label
            {
                Location = new Point(380, 20),
                Width = 150,
                Height = 20,
                Text = "Срок выполнения:"
            };

            notificationLabel = new Label
            {
                Location = new Point(550, 20),
                Width = 150,
                Height = 20,
                Text = "Уведомление:"
            };

            sortLabel = new Label
            {
                Location = new Point(20, 155),
                Width = 100,
                Height = 20,
                Text = "Сортировка:"
            };

            tasksLabel = new Label
            {
                Location = new Point(20, 195),
                Width = 200,
                Height = 25,
                Text = "Список задач:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // ========== ПОЛЯ ВВОДА ==========

            descriptionTextBox = new TextBox
            {
                Location = new Point(20, 45),
                Width = 200,
                Height = 25
            };

            priorityComboBox = new ComboBox
            {
                Location = new Point(240, 45),
                Width = 120,
                Height = 25,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            priorityComboBox.Items.AddRange(new object[] { "Низкий", "Средний", "Высокий" });

            deadlinePicker = new DateTimePicker
            {
                Location = new Point(380, 45),
                Width = 150,
                Height = 25,
                Format = DateTimePickerFormat.Short,
                ShowUpDown = false
            };

            notificationComboBox = new ComboBox
            {
                Location = new Point(550, 45),
                Width = 150,
                Height = 25,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            notificationComboBox.Items.AddRange(new object[] { "Нет", "За 10 минут", "За 1 час", "За 1 день" });
            notificationComboBox.SelectedIndex = 0;

            // ========== КНОПКИ УПРАВЛЕНИЯ ==========

            addTaskButton = new Button
            {
                Location = new Point(20, 85),
                Text = "Добавить",
                Width = 120,
                Height = 30
            };
            addTaskButton.Click += AddTaskButton_Click;

            removeTaskButton = new Button
            {
                Location = new Point(160, 85),
                Text = "Удалить",
                Width = 120,
                Height = 30
            };
            removeTaskButton.Click += RemoveTaskButton_Click;

            toggleCompletionButton = new Button
            {
                Location = new Point(300, 85),
                Text = "Отметить выполненной",
                Width = 180,
                Height = 30
            };
            toggleCompletionButton.Click += ToggleCompletionButton_Click;

            setNotificationButton = new Button
            {
                Location = new Point(500, 85),
                Text = "Установить уведомление",
                Width = 200,
                Height = 30
            };
            setNotificationButton.Click += SetNotificationButton_Click;

            // ========== СОРТИРОВКА ==========

            sortByPriorityComboBox = new ComboBox
            {
                Location = new Point(120, 152),
                Width = 180,
                Height = 25,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            sortByPriorityComboBox.Items.Add("По приоритету");
            sortByPriorityComboBox.SelectedIndex = 0;

            sortByPriorityButton = new Button
            {
                Location = new Point(320, 150),
                Text = "Сортировать",
                Width = 120,
                Height = 30
            };
            sortByPriorityButton.Click += SortByPriorityButton_Click;

            // ========== СПИСОК ЗАДАЧ ==========

            tasksListBox = new ListBox
            {
                Location = new Point(20, 225),
                Width = 700,
                Height = 360,
                Font = new Font("Consolas", 9)
            };
            tasksListBox.SelectedIndexChanged += TasksListBox_SelectedIndexChanged;

            // ========== ДОБАВЛЕНИЕ КОНТРОЛОВ ==========

            this.Controls.Add(descriptionLabel);
            this.Controls.Add(priorityLabel);
            this.Controls.Add(deadlineLabel);
            this.Controls.Add(notificationLabel);
            this.Controls.Add(sortLabel);
            this.Controls.Add(tasksLabel);

            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(priorityComboBox);
            this.Controls.Add(deadlinePicker);
            this.Controls.Add(notificationComboBox);
            this.Controls.Add(addTaskButton);
            this.Controls.Add(removeTaskButton);
            this.Controls.Add(toggleCompletionButton);
            this.Controls.Add(setNotificationButton);
            this.Controls.Add(sortByPriorityComboBox);
            this.Controls.Add(sortByPriorityButton);
            this.Controls.Add(tasksListBox);
        }

        private void InitializeNotificationTimer()
        {
            notificationTimer = new Timer();
            notificationTimer.Interval = 60000; // Проверка каждую минуту
            notificationTimer.Tick += NotificationTimer_Tick;
            notificationTimer.Start();
        }

        private void NotificationTimer_Tick(object sender, EventArgs e)
        {
            var tasksNeedingNotification = taskManager.GetTasksNeedingNotification();
            foreach (var task in tasksNeedingNotification)
            {
                ShowNotification(task);
                taskManager.MarkNotificationAsShown(task);
            }
            UpdateTasksList();
        }

        private void ShowNotification(TaskWithPriorityy task)
        {
            TimeSpan timeUntilDeadline = task.Deadline - DateTime.Now;
            string timeText = GetTimeText(timeUntilDeadline);

            MessageBox.Show(
                $"Задача: {task.Description}\n" +
                $"Приоритет: {task.Priority}\n" +
                $"Осталось времени: {timeText}",
                "Уведомление о задаче",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private string GetTimeText(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
                return $"{(int)timeSpan.TotalDays} дн.";
            else if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours} ч.";
            else
                return $"{(int)timeSpan.TotalMinutes} мин.";
        }

        private void UpdateTasksList()
        {
            tasksListBox.Items.Clear();
            foreach (var task in taskManager.Tasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ ]";
                string notification = task.NotificationTime != NotificationTime.ForNo ? "🔔" : "  ";
                tasksListBox.Items.Add($"{status} {notification} {task.Description} (Приоритет: {task.Priority}, Срок: {task.Deadline.ToString("dd.MM.yyyy HH:mm")})");
            }
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                MessageBox.Show("Введите описание задачи!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (priorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите приоритет!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Priority priority = (Priority)Enum.Parse(typeof(Priority), priorityComboBox.SelectedItem.ToString());
            DateTime deadline = deadlinePicker.Value;
            NotificationTime notificationTime = GetNotificationTimeFromComboBox();

            TaskWithPriorityy newTask = new TaskWithPriorityy(descriptionTextBox.Text, priority, deadline);
            newTask.NotificationTime = notificationTime;

            try
            {
                taskManager.AddTask(newTask);
                descriptionTextBox.Clear();
                priorityComboBox.SelectedIndex = -1;
                notificationComboBox.SelectedIndex = 0;
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private NotificationTime GetNotificationTimeFromComboBox()
        {
            switch (notificationComboBox.SelectedIndex)
            {
                case 1: return NotificationTime.For10min;
                case 2: return NotificationTime.For60min;
                case 3: return NotificationTime.For24h;
                default: return NotificationTime.ForNo;
            }
        }

        private void SetNotificationButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для установки уведомления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = tasksListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < taskManager.Tasks.Count)
            {
                NotificationTime notificationTime = GetNotificationTimeFromComboBox();

                try
                {
                    taskManager.SetNotificationTime(taskManager.Tasks[selectedIndex], notificationTime);
                    UpdateTasksList();
                    MessageBox.Show("Уведомление установлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TasksListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex >= 0 && tasksListBox.SelectedIndex < taskManager.Tasks.Count)
            {
                TaskWithPriorityy selectedTask = taskManager.Tasks[tasksListBox.SelectedIndex];
                descriptionTextBox.Text = selectedTask.Description;
                priorityComboBox.SelectedItem = selectedTask.Priority.ToString();
                deadlinePicker.Value = selectedTask.Deadline;

                // Установка выбора уведомления
                switch (selectedTask.NotificationTime)
                {
                    case NotificationTime.For10min:
                        notificationComboBox.SelectedIndex = 1;
                        break;
                    case NotificationTime.For60min:
                        notificationComboBox.SelectedIndex = 2;
                        break;
                    case NotificationTime.For24h:
                        notificationComboBox.SelectedIndex = 3;
                        break;
                    default:
                        notificationComboBox.SelectedIndex = 0;
                        break;
                }
            }
        }

        private void RemoveTaskButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = tasksListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < taskManager.Tasks.Count)
            {
                try
                {
                    var taskToRemove = taskManager.Tasks[selectedIndex];
                    taskManager.RemoveTask(taskToRemove);
                    UpdateTasksList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ToggleCompletionButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для изменения статуса!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = tasksListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < taskManager.Tasks.Count)
            {
                try
                {
                    var taskToToggle = taskManager.Tasks[selectedIndex];
                    taskManager.ToggleTaskCompletion(taskToToggle);
                    UpdateTasksList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SortByPriorityButton_Click(object sender, EventArgs e)
        {
            var sortedTasks = taskManager.SortTasksByPriority();
            tasksListBox.Items.Clear();
            foreach (var task in sortedTasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ ]";
                string notification = task.NotificationTime != NotificationTime.ForNo ? "🔔" : "  ";
                tasksListBox.Items.Add($"{status} {notification} {task.Description} (Приоритет: {task.Priority}, Срок: {task.Deadline.ToString("dd.MM.yyyy HH:mm")})");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            notificationTimer?.Stop();
            notificationTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}