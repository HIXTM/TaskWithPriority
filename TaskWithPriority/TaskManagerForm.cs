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

		// Заголовки (Labels)
		private Label descriptionLabel;
		private Label priorityLabel;
		private Label deadlineLabel;
		private Label sortLabel;
		private Label tasksLabel;

		// Поля ввода
		private TextBox descriptionTextBox;
		private ComboBox priorityComboBox;
		private DateTimePicker deadlinePicker;

		// Кнопки
		private Button addTaskButton;
		private Button removeTaskButton;
		private Button toggleCompletionButton;
		private ComboBox sortByPriorityComboBox;
		private Button sortByPriorityButton;

		// Список задач
		private ListBox tasksListBox;

		public TaskManagerForm()
		{
			this.Text = "Управление задачами с приоритетом";
			this.Width = 700;
			this.Height = 600;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;

			// ========== ЗАГОЛОВКИ ==========

			descriptionLabel = new Label
			{
				Location = new Point(20, 20),
				Width = 200,
				Height = 20,
				Text = "Описание задачи:",
				Font = new Font("Segoe UI", 9, FontStyle.Regular)
			};

			priorityLabel = new Label
			{
				Location = new Point(240, 20),
				Width = 120,
				Height = 20,
				Text = "Приоритет:",
				Font = new Font("Segoe UI", 9, FontStyle.Regular)
			};

			deadlineLabel = new Label
			{
				Location = new Point(380, 20),
				Width = 200,
				Height = 20,
				Text = "Срок выполнения:",
				Font = new Font("Segoe UI", 9, FontStyle.Regular)
			};

			// ========== ПОЛЯ ВВОДА ==========

			descriptionTextBox = new TextBox
			{
				Location = new Point(20, 45),
				Width = 200,
				Height = 25,
				Font = new Font("Segoe UI", 9)
			};

			priorityComboBox = new ComboBox
			{
				Location = new Point(240, 45),
				Width = 120,
				Height = 25,
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Segoe UI", 9)
			};
			priorityComboBox.Items.AddRange(new object[] { "Низкий", "Средний", "Высокий" });

			deadlinePicker = new DateTimePicker
			{
				Location = new Point(380, 45),
				Width = 280,
				Height = 25,
				Font = new Font("Segoe UI", 9),
				Format = DateTimePickerFormat.Long
			};

			// ========== КНОПКИ УПРАВЛЕНИЯ (ОБЫЧНЫЙ СТИЛЬ) ==========

			addTaskButton = new Button
			{
				Location = new Point(20, 85),
				Text = "Добавить",
				Width = 120,
				Height = 30,
				Font = new Font("Segoe UI", 9)
			};
			addTaskButton.Click += AddTaskButton_Click;

			removeTaskButton = new Button
			{
				Location = new Point(160, 85),
				Text = "Удалить",
				Width = 120,
				Height = 30,
				Font = new Font("Segoe UI", 9)
			};
			removeTaskButton.Click += RemoveTaskButton_Click;

			toggleCompletionButton = new Button
			{
				Location = new Point(300, 85),
				Text = "Отметить выполненной",
				Width = 180,
				Height = 30,
				Font = new Font("Segoe UI", 9)
			};
			toggleCompletionButton.Click += ToggleCompletionButton_Click;

			// ========== СОРТИРОВКА ==========

			sortLabel = new Label
			{
				Location = new Point(20, 135),
				Width = 100,
				Height = 20,
				Text = "Сортировка:",
				Font = new Font("Segoe UI", 9, FontStyle.Regular)
			};

			sortByPriorityComboBox = new ComboBox
			{
				Location = new Point(120, 132),
				Width = 180,
				Height = 25,
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Segoe UI", 9)
			};
			sortByPriorityComboBox.Items.Add("По приоритету");
			sortByPriorityComboBox.SelectedIndex = 0;

			sortByPriorityButton = new Button
			{
				Location = new Point(320, 130),
				Text = "Сортировать",
				Width = 120,
				Height = 30,
				Font = new Font("Segoe UI", 9)
			};
			sortByPriorityButton.Click += SortByPriorityButton_Click;

			// ========== СПИСОК ЗАДАЧ ==========

			tasksLabel = new Label
			{
				Location = new Point(20, 175),
				Width = 200,
				Height = 25,
				Text = "Список задач:",
				Font = new Font("Segoe UI", 10, FontStyle.Bold)
			};

			tasksListBox = new ListBox
			{
				Location = new Point(20, 205),
				Width = 640,
				Height = 320,
				Font = new Font("Consolas", 9)
			};

			// ========== ДОБАВЛЕНИЕ КОНТРОЛОВ НА ФОРМУ ==========

			this.Controls.Add(descriptionLabel);
			this.Controls.Add(priorityLabel);
			this.Controls.Add(deadlineLabel);
			this.Controls.Add(sortLabel);
			this.Controls.Add(tasksLabel);

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
				string status = task.IsCompleted ? "[X]" : "[ ]";
				tasksListBox.Items.Add($"{status} {task.Description} (Приоритет: {task.Priority})");
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
			TaskWithPriorityy newTask = new TaskWithPriorityy(descriptionTextBox.Text, priority, deadline);

			try
			{
				taskManager.AddTask(newTask);
				descriptionTextBox.Clear();
				priorityComboBox.SelectedIndex = -1;
				UpdateTasksList();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				tasksListBox.Items.Add($"{status} {task.Description} (Приоритет: {task.Priority})");
			}
		}
	}
}