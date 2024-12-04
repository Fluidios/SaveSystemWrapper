using NUnit.Framework;
using Game.SaveSystem.Core;

namespace Game.SaveSystem.Tests
{
    [TestFixture]
    public class SaveManagerTest
    {
        [Test]
        public void Save_ShouldStoreValue()
        {
            // Arrange
            string key = "testKey";
            int value = 42;

            // Act
            SaveSystem.Core.SaveManager.Save(key, value);

            // Assert
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, 0);
            Assert.AreEqual(value, loadedValue);
        }

        [Test]
        public void Load_ShouldReturnDefaultValue_WhenKeyDoesNotExist()
        {
            // Arrange
            string key = "nonExistentKey";
            int defaultValue = 99;

            // Act
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, defaultValue);

            // Assert
            Assert.AreEqual(defaultValue, loadedValue);
        }
        
        [Test]
        public void CreateBackup_ShouldRestoreDeletedKey()
        {
            // Arrange
            string key = "backupTestKey";
            int value = 123;
            SaveSystem.Core.SaveManager.Save(key, value);

            // Act
            SaveSystem.Core.SaveManager.CreateBackup(null);

            // Assert
            SaveSystem.Core.SaveManager.DeleteKey(key, null);
            SaveSystem.Core.SaveManager.RestoreBackup(null);
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, 0);
            Assert.AreEqual(value, loadedValue);
        }

        [Test]
        public void RestoreBackup_ShouldRestorePreviousState()
        {
            // Arrange
            string key = "restoreTestKey";
            int initialValue = 456;
            int newValue = 789;
            SaveSystem.Core.SaveManager.Save(key, initialValue);
            SaveSystem.Core.SaveManager.CreateBackup(null);
            SaveSystem.Core.SaveManager.Save(key, newValue);

            // Act
            SaveSystem.Core.SaveManager.RestoreBackup(null);

            // Assert
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, 0);
            Assert.AreEqual(initialValue, loadedValue);
        }
        [Test]
        public void Save_ShouldOverwriteExistingValue()
        {
            // Arrange
            string key = "overwriteTestKey";
            int initialValue = 111;
            int newValue = 222;
            SaveSystem.Core.SaveManager.Save(key, initialValue);

            // Act
            SaveSystem.Core.SaveManager.Save(key, newValue);

            // Assert
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, 0);
            Assert.AreEqual(newValue, loadedValue);
        }

        [Test]
        public void DeleteKey_ShouldRemoveValue()
        {
            // Arrange
            string key = "deleteTestKey";
            int value = 333;
            SaveSystem.Core.SaveManager.Save(key, value);

            // Act
            SaveSystem.Core.SaveManager.DeleteKey(key, null);

            // Assert
            int loadedValue = SaveSystem.Core.SaveManager.Load(key, 0);
            Assert.AreEqual(0, loadedValue);
        }

        [Test]
        public void DeleteAll_ShouldRemoveAllValues()
        {
            // Arrange
            string key1 = "deleteAllTestKey1";
            string key2 = "deleteAllTestKey2";
            int value1 = 444;
            int value2 = 555;
            SaveSystem.Core.SaveManager.Save(key1, value1);
            SaveSystem.Core.SaveManager.Save(key2, value2);

            // Act
            SaveSystem.Core.SaveManager.DeleteAll(null);

            // Assert
            int loadedValue1 = SaveSystem.Core.SaveManager.Load(key1, 0);
            int loadedValue2 = SaveSystem.Core.SaveManager.Load(key2, 0);
            Assert.AreEqual(0, loadedValue1);
            Assert.AreEqual(0, loadedValue2);
        }
        [Test]
        public void SaveSlot_ShouldMatter()
        {
            // Arrange
            string key = "slotTestKey";
            int value1 = 666;
            int value2 = 777;
            SaveSystem.Core.SaveManager.Save(key, value1, new SMSettings("slotTest.json"));
            SaveSystem.Core.SaveManager.Save(key, value2, new SMSettings("slotTest.json") { SaveSlot = 1 });

            // Act
            int loadedValue1 = SaveSystem.Core.SaveManager.Load(key, 0, new SMSettings("slotTest.json"));
            int loadedValue2 = SaveSystem.Core.SaveManager.Load(key, 0, new SMSettings("slotTest.json") { SaveSlot = 1 });

            // Assert
            Assert.AreEqual(value1, loadedValue1);
            Assert.AreEqual(value2, loadedValue2);
            Assert.AreNotEqual(loadedValue1, loadedValue2);
        }
    }
}