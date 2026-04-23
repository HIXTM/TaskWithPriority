using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManager.Tests
{
    [TestClass]
    public class TaskManagerWithPriorityTests
    {
        private const string TestFilePath = "tasks.txt";

        // Удаляем файл перед каждым тестом 
        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        // Удаляем файл после каждого теста
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TestMethod]
        public void Constructor_InitializesEmptyList()
        {
            // Act
            var manager = new TaskManagerWithPriority();

            // Assert
            Assert.IsNotNull(manager.Tasks);
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void AddTask_AddsToList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Тестовая задача", Priority.Высокий, DateTime.Now);

            // Act
            manager.AddTask(task);

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual(task, manager.Tasks[0]);
        }

        [TestMethod]
        public void AddTask_SavesToFile()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);

            // Act
            manager.AddTask(task);

            // Assert
            Assert.IsTrue(File.Exists(TestFilePath));
        }

        [TestMethod]
        public void AddTask_AddsMultipleTasks()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task1 = new TaskWithPriorityy("Задача 1", Priority.Низкий, DateTime.Now);
            var task2 = new TaskWithPriorityy("Задача 2", Priority.Высокий, DateTime.Now);

            // Act
            manager.AddTask(task1);
            manager.AddTask(task2);

            // Assert
            Assert.AreEqual(2, manager.Tasks.Count);
        }

        [TestMethod]
        public void RemoveTask_RemovesFromList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            manager.AddTask(task);

            // Act
            manager.RemoveTask(task);

            // Assert
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void RemoveTask_TaskNotFound_NoError()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task1 = new TaskWithPriorityy("Задача 1", Priority.Низкий, DateTime.Now);
            var task2 = new TaskWithPriorityy("Задача 2", Priority.Высокий, DateTime.Now);
            manager.AddTask(task1);

            // Act
            manager.RemoveTask(task2);

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
        }

        [TestMethod]
        public void RemoveTask_SavesToFile()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            manager.AddTask(task);

            // Act
            manager.RemoveTask(task);

            // Assert
            Assert.IsTrue(File.Exists(TestFilePath));
        }

        [TestMethod]
        public void ToggleTaskCompletion_MarksAsCompleted()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            manager.AddTask(task);

            // Act
            manager.ToggleTaskCompletion(task);

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void ToggleTaskCompletion_MarksAsNotCompleted()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            task.IsCompleted = true;
            manager.AddTask(task);

            // Act
            manager.ToggleTaskCompletion(task);

            // Assert
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void ToggleTaskCompletion_SavesToFile()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            manager.AddTask(task);

            // Act
            manager.ToggleTaskCompletion(task);

            // Assert
            Assert.IsTrue(File.Exists(TestFilePath));
        }

        [TestMethod]
        public void SortTasksByPriority_EmptyList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();

            // Act
            var sortedTasks = manager.SortTasksByPriority();

            // Assert
            Assert.AreEqual(0, sortedTasks.Count);
        }

        [TestMethod]
        public void LoadTasks_FromFile()
        {
            // Arrange
            var deadline = new DateTime(2026, 5, 1, 10, 30, 0);
            File.WriteAllLines(TestFilePath, new[]
            {
                $"Задача 1|2|False|{deadline.ToString("yyyy-MM-dd HH: mm:ss")}",
                $"Задача 2|1|True|{deadline.ToString("yyyy-MM-dd HH: mm:ss")}"
            });

            // Act
            var manager = new TaskManagerWithPriority();

            // Assert
            Assert.AreEqual(2, manager.Tasks.Count);
            Assert.AreEqual("Задача 1", manager.Tasks[0].Description);
            Assert.AreEqual(Priority.Высокий, manager.Tasks[0].Priority);
            Assert.IsFalse(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        public void LoadTasks_FileNotFound_EmptyList()
        {
            // Act
            var manager = new TaskManagerWithPriority();

            // Assert
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void LoadTasks_SkipsInvalidData()
        {
            // Arrange
            var deadline = new DateTime(2026, 5, 1, 10, 30, 0);
            File.WriteAllLines(TestFilePath, new[]
            {
                $"Задача 1|2|False|{deadline.ToString("yyyy-MM-dd HH: mm:ss")}",
                "Неверные данные",
                $"Задача 2|1|True|{deadline.ToString("yyyy-MM-dd HH: mm:ss")}"
            });

            // Act
            var manager = new TaskManagerWithPriority();

            // Assert
            Assert.AreEqual(2, manager.Tasks.Count);
        }

        [TestMethod]
        public void Tasks_ReturnsList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            manager.AddTask(task);

            // Act
            var tasks = manager.Tasks;

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ThrowsArgumentException()
        {
            // Act
            new TaskWithPriorityy("", Priority.Низкий, DateTime.Now);
        }
    }
}