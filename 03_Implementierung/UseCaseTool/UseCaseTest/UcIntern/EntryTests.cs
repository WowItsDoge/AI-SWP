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
    }
}
