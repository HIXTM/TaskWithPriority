using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TaskWithPriority;

namespace TaskManager.Tests
{
    [TestClass]
    public class TaskManagerFormTests
    {
        private const string TestFilePath = "tasks.txt";
        private TaskManagerForm form;

        // Удаляем файл перед каждым тестом
        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            form = new TaskManagerForm();
        }

        // Закрываем форму и удаляем файл после теста
        [TestCleanup]
        public void TestCleanup()
        {
            form?.Dispose();

            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TestMethod]
        public void AddTaskButton_ClearsTextBox()
        {
            // Arrange
            var descriptionTextBox = GetPrivateField<TextBox>("descriptionTextBox");
            var priorityComboBox = GetPrivateField<ComboBox>("priorityComboBox");
            var addTaskButton = GetPrivateField<Button>("addTaskButton");

            descriptionTextBox.Text = "Тестовая задача";
            priorityComboBox.SelectedItem = "Средний";

            // Act
            InvokePrivateMethod("AddTaskButton_Click", addTaskButton, EventArgs.Empty);

            // Assert
            Assert.AreEqual("", descriptionTextBox.Text);
        }

        [TestMethod]
        public void AddTaskButton_UpdatesList()
        {
            // Arrange
            var descriptionTextBox = GetPrivateField<TextBox>("descriptionTextBox");
            var priorityComboBox = GetPrivateField<ComboBox>("priorityComboBox");
            var addTaskButton = GetPrivateField<Button>("addTaskButton");
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");

            descriptionTextBox.Text = "Новая задача";
            priorityComboBox.SelectedItem = "Низкий";

            int initialListCount = tasksListBox.Items.Count;

            // Act
            InvokePrivateMethod("AddTaskButton_Click", addTaskButton, EventArgs.Empty);

            // Assert
            Assert.AreEqual(initialListCount + 1, tasksListBox.Items.Count);
        }

        [TestMethod]
        public void RemoveTaskButton_NoSelection_NoRemove()
        {
            // Arrange
            var removeTaskButton = GetPrivateField<Button>("removeTaskButton");
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            // Добавляем задачу
            taskManager.AddTask(new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now));
            InvokePrivateMethod("UpdateTasksList");

            tasksListBox.SelectedIndex = -1;
            int initialCount = taskManager.Tasks.Count;

            // Act
            InvokePrivateMethod("RemoveTaskButton_Click", removeTaskButton, EventArgs.Empty);

            // Assert
            Assert.AreEqual(initialCount, taskManager.Tasks.Count);
        }

        [TestMethod]
        public void ToggleCompletionButton_NoSelection_NoChange()
        {
            // Arrange
            var toggleButton = GetPrivateField<Button>("toggleCompletionButton");
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            taskManager.AddTask(task);
            InvokePrivateMethod("UpdateTasksList");

            tasksListBox.SelectedIndex = -1;
            bool initialStatus = task.IsCompleted;

            // Act
            InvokePrivateMethod("ToggleCompletionButton_Click", toggleButton, EventArgs.Empty);

            // Assert
            Assert.AreEqual(initialStatus, task.IsCompleted);
        }

        [TestMethod]
        public void SortByPriorityButton_SortsTasks()
        {
            // Arrange
            var sortButton = GetPrivateField<Button>("sortByPriorityButton");
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            taskManager.AddTask(new TaskWithPriorityy("Низкий приоритет", Priority.Низкий, DateTime.Now));
            taskManager.AddTask(new TaskWithPriorityy("Высокий приоритет", Priority.Высокий, DateTime.Now));
            taskManager.AddTask(new TaskWithPriorityy("Средний приоритет", Priority.Средний, DateTime.Now));
            InvokePrivateMethod("UpdateTasksList");

            // Act
            InvokePrivateMethod("SortByPriorityButton_Click", sortButton, EventArgs.Empty);

            // Assert
            Assert.AreEqual(3, tasksListBox.Items.Count);
            string firstItem = tasksListBox.Items[0].ToString();
            Assert.IsTrue(firstItem.Contains("Высокий приоритет"));
        }

        [TestMethod]
        public void UpdateTasksList_DisplaysTasks()
        {
            // Arrange
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            taskManager.AddTask(new TaskWithPriorityy("Задача 1", Priority.Высокий, DateTime.Now));
            taskManager.AddTask(new TaskWithPriorityy("Задача 2", Priority.Средний, DateTime.Now));

            // Act
            InvokePrivateMethod("UpdateTasksList");

            // Assert
            Assert.AreEqual(2, tasksListBox.Items.Count);
        }

        [TestMethod]
        public void UpdateTasksList_ShowCompletedStatus()
        {
            // Arrange
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            var task = new TaskWithPriorityy("Завершенная задача", Priority.Высокий, DateTime.Now);
            task.IsCompleted = true;
            taskManager.AddTask(task);

            // Act
            InvokePrivateMethod("UpdateTasksList");

            // Assert
            string listItem = tasksListBox.Items[0].ToString();
            Assert.IsTrue(listItem.Contains("[X]"));
        }

        [TestMethod]
        public void UpdateTasksList_ShowNotCompletedStatus()
        {
            // Arrange
            var tasksListBox = GetPrivateField<ListBox>("tasksListBox");
            var taskManager = GetPrivateField<TaskManagerWithPriority>("taskManager");

            var task = new TaskWithPriorityy("Незавершенная задача", Priority.Низкий, DateTime.Now);
            taskManager.AddTask(task);

            // Act
            InvokePrivateMethod("UpdateTasksList");

            // Assert
            string listItem = tasksListBox.Items[0].ToString();
            Assert.IsTrue(listItem.Contains("[ )"));
        }

        // Вспомогательные методы для доступа к приватным полям и методам
        private T GetPrivateField<T>(string fieldName)
        {
            var field = typeof(TaskManagerForm).GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(form);
        }

        private void InvokePrivateMethod(string methodName, params object[] parameters)
        {
            var method = typeof(TaskManagerForm).GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(form, parameters);
        }
    }
}