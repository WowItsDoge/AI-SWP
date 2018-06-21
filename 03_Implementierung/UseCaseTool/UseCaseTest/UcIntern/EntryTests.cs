// <copyright file="EntryTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Entry{T}"/> class.
    /// </summary>
    [TestFixture]
    public class EntryTests
    {
        /// <summary>
        /// Creates a new entry and tests if the column index is set correctly.
        /// </summary>
        [Test]
        public void GetColumnIndex()
        {
            // Arrange
            int columnIndex = 5;
            Entry<string> entry = new Entry<string>(columnIndex, null);

            // Act

            // Assert
            Assert.AreEqual(columnIndex, entry.ColumnIndex);
        }

        /// <summary>
        /// Creates a new entry and tests if the content is set correctly.
        /// </summary>
        [Test]
        public void GetContent()
        {
            // Arrange
            string content = "Test string";
            Entry<string> entry = new Entry<string>(0, content);

            // Act

            // Assert
            Assert.AreSame(content, entry.Content);
        }

        /// <summary>
        /// Creates a new entry and sets a new content.
        /// </summary>
        [Test]
        public void SetContent()
        {
            // Arrange
            string oldContent = "Test string",
                newContent = "New test string";
            Entry<string> entry = new Entry<string>(0, oldContent);

            // Act
            entry.Content = newContent;

            // Assert
            Assert.AreSame(newContent, entry.Content);
        }

        /// <summary>
        /// Creates a new entry and a readonly copy of it.
        /// It is tested if the column index is like in the original entry.
        /// </summary>
        [Test]
        public void GetReadonlyColumnIndex()
        {
            // Arrange
            int columnIndex = 6;
            Entry<string> entry = new Entry<string>(columnIndex, null);

            // Act
            Entry<string> readonlyEntry = entry.AsReadonly();

            // Assert
            Assert.AreEqual(columnIndex, readonlyEntry.ColumnIndex);
        }

        /// <summary>
        /// Creates a new entry and creates a readonly copy of it.
        /// Test if the IsReadonly property is set correctly.
        /// </summary>
        [Test]
        public void TestIsReadonly()
        {
            // Arrange
            Entry<object> entry = new Entry<object>(0, null);

            // Act
            Entry<object> readonlyEntry = entry.AsReadonly();

            // Assert
            Assert.AreEqual(false, entry.IsReadonly);
            Assert.AreEqual(true, readonlyEntry.IsReadonly);
        }

        /// <summary>
        /// Creates a new entry and a readonly copy of it.
        /// It is tested if the content matches the content from the original entry.
        /// </summary>
        [Test]
        public void GetReadonlyContent()
        {
            // Arrange
            string content = "Test string";
            Entry<string> entry = new Entry<string>(0, content);

            // Act
            Entry<string> readonlyEntry = entry.AsReadonly();

            // Assert
            Assert.AreSame(content, readonlyEntry.Content);
        }

        /// <summary>
        /// Creates a new entry and a readonly copy of it.
        /// It is tested that the content can't be set.
        /// </summary>
        [Test]
        public void SetReadonlyContent()
        {
            // Arrange
            string oldContent = "Test string",
                newContent = "New test string";
            Entry<string> entry = new Entry<string>(0, oldContent);

            // Act
            Entry<string> readonlyEntry = entry.AsReadonly();

            // Asset
            Assert.Catch(
                typeof(InvalidOperationException),
                () =>
                {
                    readonlyEntry.Content = newContent;
                });
        }

        /// <summary>
        /// Creates a row with less than zero columns.
        /// Tests if this is invalid.
        /// </summary>
        [Test]
        public void EntryWithNegativeColumnIndex()
        {
            // Arrange
            int columnIndex = -1;

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    Entry<object> entry = new Entry<object>(columnIndex, null);
                });
        }

        /// <summary>
        /// Compares an entry with an object instance.
        /// </summary>
        [Test]
        public void CompareEntryToObject()
        {
            // Arrange
            Entry<object> e = new Entry<object>(0, null);

            // Act

            // Assert
            Assert.IsFalse(e.Equals(new object()));
        }

        /// <summary>
        /// Compares an entry with null.
        /// </summary>
        [Test]
        public void CompareEntryToNull()
        {
            // Arrange
            Entry<object> e = new Entry<object>(0, null);

            // Act

            // Assert
            Assert.IsFalse(e.Equals(null));
        }

        /// <summary>
        /// Compares two entries that are equal.
        /// </summary>
        [Test]
        public void CompareEqualEntries()
        {
            // Arrange
            Entry<object> e1 = new Entry<object>(0, null),
                e2 = new Entry<object>(0, null);

            // Act

            // Assert
            Assert.IsTrue(e1.Equals(e2));
        }

        /// <summary>
        /// Compares two entries with different column index.
        /// </summary>
        [Test]
        public void CompareEntriesDifferentColumnIndex()
        {
            // Arrange
            Entry<object> e1 = new Entry<object>(1, null),
                e2 = new Entry<object>(2, null);

            // Act

            // Assert
            Assert.IsFalse(e1.Equals(e2));
        }

        /// <summary>
        /// Compares two entries with different content.
        /// </summary>
        [Test]
        public void CompareEntriesDifferentContent()
        {
            // Arrange
            Entry<object> e1 = new Entry<object>(3, new object()),
                e2 = new Entry<object>(3, new object());

            // Act

            // Assert
            Assert.IsFalse(e1.Equals(e2));
        }

        /// <summary>
        /// Tests the hash of two differing entry objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            Entry<object> e1 = new Entry<object>(7, new object()),
                e2 = new Entry<object>(3, null);

            // Act

            // Assert
            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }
    }
}
