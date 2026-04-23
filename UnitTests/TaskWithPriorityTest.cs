using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TaskManager.Tests
{
    [TestClass]
    public class TaskWithPriorityyTests
    {
        [TestMethod]
        public void Constructor_SetsProperties()
        {
            // Arrange
            string description = "Завершить проект";
            Priority priority = Priority.Высокий;
            DateTime deadline = new DateTime(2026, 5, 1);

            // Act
            var task = new TaskWithPriorityy(description, priority, deadline);

            // Assert
            Assert.AreEqual(description, task.Description);
            Assert.AreEqual(priority, task.Priority);
            Assert.AreEqual(deadline, task.Deadline);
        }

        [TestMethod]
        public void Task_IsNotCompletedByDefault()
        {
            // Arrange
            string description = "Тестовая задача";
            Priority priority = Priority.Средний;
            DateTime deadline = DateTime.Now.AddDays(7);

            // Act
            var task = new TaskWithPriorityy(description, priority, deadline);

            // Assert
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Constructor_SetsLowPriority()
        {
            // Arrange
            string description = "Задача с низким приоритетом";
            Priority priority = Priority.Низкий;
            DateTime deadline = DateTime.Now.AddDays(1);

            // Act
            var task = new TaskWithPriorityy(description, priority, deadline);

            // Assert
            Assert.AreEqual(Priority.Низкий, task.Priority);
        }

        [TestMethod]
        public void Constructor_SetsMediumPriority()
        {
            // Arrange
            string description = "Задача со средним приоритетом";
            Priority priority = Priority.Средний;
            DateTime deadline = DateTime.Now.AddDays(1);

            // Act
            var task = new TaskWithPriorityy(description, priority, deadline);

            // Assert
            Assert.AreEqual(Priority.Средний, task.Priority);
        }

        [TestMethod]
        public void Constructor_SetsHighPriority()
        {
            // Arrange
            string description = "Задача с высоким приоритетом";
            Priority priority = Priority.Высокий;
            DateTime deadline = DateTime.Now.AddDays(1);

            // Act
            var task = new TaskWithPriorityy(description, priority, deadline);

            // Assert
            Assert.AreEqual(Priority.Высокий, task.Priority);
        }

        [TestMethod]
        public void IsCompleted_CanBeSetTrue()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Низкий, DateTime.Now);

            // Act
            task.IsCompleted = true;

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void IsCompleted_CanBeSetFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Низкий, DateTime.Now);
            task.IsCompleted = true;

            // Act
            task.IsCompleted = false;

            // Assert
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Description_CanBeChanged()
        {
            // Arrange
            var task = new TaskWithPriorityy("Старое описание", Priority.Средний, DateTime.Now);
            string newDescription = "Новое описание";

            // Act
            task.Description = newDescription;

            // Assert
            Assert.AreEqual(newDescription, task.Description);
        }

        [TestMethod]
        public void Priority_CanBeChanged()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Низкий, DateTime.Now);
            Priority newPriority = Priority.Высокий;

            // Act
            task.Priority = newPriority;

            // Assert
            Assert.AreEqual(newPriority, task.Priority);
        }

        [TestMethod]
        public void Deadline_CanBeChanged()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now);
            DateTime newDeadline = DateTime.Now.AddDays(30);

            // Act
            task.Deadline = newDeadline;

            // Assert
            Assert.AreEqual(newDeadline, task.Deadline);
        }

        [TestMethod]
        public void Constructor_AcceptsPastDeadline()
        {
            // Arrange
            string description = "Просроченная задача";
            Priority priority = Priority.Высокий;
            DateTime pastDeadline = DateTime.Now.AddDays(-5);

            // Act
            var task = new TaskWithPriorityy(description, priority, pastDeadline);

            // Assert
            Assert.AreEqual(pastDeadline, task.Deadline);
            Assert.IsTrue(task.Deadline < DateTime.Now);
        }

        [TestMethod]
        public void Constructor_AcceptsFutureDeadline()
        {
            // Arrange
            string description = "Будущая задача";
            Priority priority = Priority.Средний;
            DateTime futureDeadline = DateTime.Now.AddDays(10);

            // Act
            var task = new TaskWithPriorityy(description, priority, futureDeadline);

            // Assert
            Assert.AreEqual(futureDeadline, task.Deadline);
            Assert.IsTrue(task.Deadline > DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTask_ThrowsArgumentNullException()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();

            // Act
            manager.AddTask(null);
        }
    }
}