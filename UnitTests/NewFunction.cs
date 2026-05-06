using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TaskManager.Tests
{
    [TestClass]
    public class NotificationTests
    {
        private const string TestFilePath = "tasks.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TestMethod]
        public void SetNotificationTime_WhenValidTimeProvided_UpdatesNotificationTime()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Высокий, DateTime.Now.AddHours(2));
            manager.AddTask(task);

            // Act
            manager.SetNotificationTime(task, NotificationTime.For60min);

            // Assert
            Assert.AreEqual(NotificationTime.For60min, task.NotificationTime);
        }

        [TestMethod]
        public void ShouldShowNotification_WhenTimeMatches_ReturnsTrue()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now.AddMinutes(30));
            task.NotificationTime = NotificationTime.For60min;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsTrue(shouldShow);
        }

        [TestMethod]
        public void ShouldShowNotification_WhenTaskCompleted_ReturnsFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now.AddMinutes(30));
            task.NotificationTime = NotificationTime.For60min;
            task.IsCompleted = true;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsFalse(shouldShow);
        }

        [TestMethod]
        public void ShouldShowNotification_WhenNotificationAlreadyShown_ReturnsFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now.AddMinutes(30));
            task.NotificationTime = NotificationTime.For60min;
            task.NotificationShown = true;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsFalse(shouldShow);
        }

        [TestMethod]
        public void ShouldShowNotification_WhenNotificationDisabled_ReturnsFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Задача", Priority.Средний, DateTime.Now.AddMinutes(30));
            task.NotificationTime = NotificationTime.For10min;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsFalse(shouldShow);
        }

        [TestMethod]
        public void GetTasksNeedingNotification_WhenTasksNeedNotification_ReturnsCorrectList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task1 = new TaskWithPriorityy("Задача 1", Priority.Высокий, DateTime.Now.AddMinutes(30));
            task1.NotificationTime = NotificationTime.For60min;
            var task2 = new TaskWithPriorityy("Задача 2", Priority.Средний, DateTime.Now.AddDays(2));
            task2.NotificationTime = NotificationTime.For24h;
            manager.AddTask(task1);
            manager.AddTask(task2);

            // Act
            var tasksNeedingNotification = manager.GetTasksNeedingNotification();

            // Assert
            Assert.AreEqual(1, tasksNeedingNotification.Count);
            Assert.AreEqual("Задача 1", tasksNeedingNotification[0].Description);
        }

        [TestMethod]
        public void MarkNotificationAsShown_WhenCalled_SetsNotificationShownToTrue()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача", Priority.Высокий, DateTime.Now.AddHours(2));
            manager.AddTask(task);

            // Act
            manager.MarkNotificationAsShown(task);

            // Assert
            Assert.IsTrue(task.NotificationShown);
        }

        [TestMethod]
        public void SaveAndLoadTasks_WithNotificationSettings_PreservesSettings()
        {
            // Arrange
            var manager1 = new TaskManagerWithPriority();
            var task = new TaskWithPriorityy("Задача с уведомлением", Priority.Высокий, DateTime.Now.AddHours(3));
            task.NotificationTime = NotificationTime.For60min;
            manager1.AddTask(task);

            // Act
            var manager2 = new TaskManagerWithPriority();

            // Assert
            Assert.AreEqual(1, manager2.Tasks.Count);
            Assert.AreEqual(NotificationTime.For60min, manager2.Tasks[0].NotificationTime);
            Assert.AreEqual("Задача с уведомлением", manager2.Tasks[0].Description);
        }

        [TestMethod]
        public void SetNotificationTime_WhenNullTaskProvided_ThrowsArgumentNullException()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                manager.SetNotificationTime(null, NotificationTime.For60min));
        }

        [TestMethod]
        public void MarkNotificationAsShown_WhenNullTaskProvided_ThrowsArgumentNullException()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                manager.MarkNotificationAsShown(null));
        }

        [TestMethod]
        public void ShouldShowNotification_WhenDeadlinePassed_ReturnsFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Просроченная задача", Priority.Высокий, DateTime.Now.AddMinutes(-10));
            task.NotificationTime = NotificationTime.For60min;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsFalse(shouldShow);
        }

        [TestMethod]
        public void ShouldShowNotification_WhenTooFarFromDeadline_ReturnsFalse()
        {
            // Arrange
            var task = new TaskWithPriorityy("Далекая задача", Priority.Средний, DateTime.Now.AddDays(5));
            task.NotificationTime = NotificationTime.For24h;

            // Act
            bool shouldShow = task.ShouldShowNotification();

            // Assert
            Assert.IsFalse(shouldShow);
        }

        [TestMethod]
        public void GetTasksNeedingNotification_WhenNoTasks_ReturnsEmptyList()
        {
            // Arrange
            var manager = new TaskManagerWithPriority();

            // Act
            var tasksNeedingNotification = manager.GetTasksNeedingNotification();

            // Assert
            Assert.AreEqual(0, tasksNeedingNotification.Count);
        }
    }
}