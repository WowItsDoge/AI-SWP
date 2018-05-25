// <copyright file="RowTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Row{T}"/> class.
    /// </summary>
    [TestFixture]
    public class RowTests
    {
        /// <summary>
        /// Creates a new row without any set entries.
        /// Tests if every valid index returns the standard return object.
        /// </summary>
        [Test]
        public void GetStandardReturnObjectFromRowWithoutAnyEntrySet()
        {
            // Arrange
            int columnCount = 5;
            string standardReturnObject = "Standard string";
            Row<string> row = new Row<string>(columnCount, standardReturnObject);

            // Act

            // Assert
            for (int index = 0; index < columnCount; index++)
            {
                Assert.AreSame(standardReturnObject, row[index]);
            }
        }

        /// <summary>
        /// Creates a new row and sets some indices to different objects.
        /// Tests if every valid index returns the standard return object
        /// except for the set indices.
        /// </summary>
        [Test]
        public void GetContentWithPartiallyFilledRow()
        {
            // Arrange
            int columnCount = 5,
                testString1Index = 2,
                testString2Index = 4,
                testString3Index = 0;
            string standardReturnObject = "Standard string",
                testString1 = "Test string 1",
                testString2 = "Test string 2",
                testString3 = "Test string 3";
            Row<string> row = new Row<string>(columnCount, standardReturnObject);

            // Act
            row[testString1Index] = testString1;
            row[testString2Index] = testString2;
            row[testString3Index] = testString3;

            // Assert
            for (int index = 0; index < columnCount; index++)
            {
                string returnObject = row[index];

                if (index == testString1Index)
                {
                    Assert.AreSame(testString1, returnObject);
                }
                else if (index == testString2Index)
                {
                    Assert.AreSame(testString2, returnObject);
                }
                else if (index == testString3Index)
                {
                    Assert.AreSame(testString3, returnObject);
                }
                else
                {
                    Assert.AreSame(standardReturnObject, returnObject);
                }
            }
        }

        /// <summary>
        /// Creates a new row and tests indices out of range for get and set.
        /// </summary>
        [Test]
        public void AccessInvalidIndices()
        {
            // Arrange
            int columnCount = 5;
            Row<object> row = new Row<object>(columnCount, null);

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    row[-1] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    object o = row[-1];
                });

            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    row[columnCount] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    object o = row[columnCount];
                });
        }

        /// <summary>
        /// Creates a new row and creates a readonly copy of it.
        /// Test if the IsReadonly property is set correctly.
        /// </summary>
        [Test]
        public void TestIsReadonly()
        {
            // Arrange
            int columnCount = 5;
            Row<object> row = new Row<object>(columnCount, null);

            // Act
            Row<object> readonlyRow = row.AsReadonly();

            // Assert
            Assert.AreEqual(false, row.IsReadonly);
            Assert.AreEqual(true, readonlyRow.IsReadonly);
        }

        /// <summary>
        /// Creates a new row and creates a readonly copy of it.
        /// Some indices in the editable row were set to different objects.
        /// Tests if every valid index returns the standard return object
        /// except for the set indices.
        /// </summary>
        [Test]
        public void GetReadonlyContentWithPartiallyFilledRow()
        {
            // Arrange
            int columnCount = 5,
                testString1Index = 2,
                testString2Index = 4,
                testString3Index = 0;
            string standardReturnObject = "Standard string",
                testString1 = "Test string 1",
                testString2 = "Test string 2",
                testString3 = "Test string 3";
            Row<string> row = new Row<string>(columnCount, standardReturnObject);

            // Act
            row[testString1Index] = testString1;
            row[testString2Index] = testString2;
            row[testString3Index] = testString3;
            Row<string> readonlyRow = row.AsReadonly();

            // Assert
            for (int index = 0; index < columnCount; index++)
            {
                string returnObject = readonlyRow[index];

                if (index == testString1Index)
                {
                    Assert.AreSame(testString1, returnObject);
                }
                else if (index == testString2Index)
                {
                    Assert.AreSame(testString2, returnObject);
                }
                else if (index == testString3Index)
                {
                    Assert.AreSame(testString3, returnObject);
                }
                else
                {
                    Assert.AreSame(standardReturnObject, returnObject);
                }
            }
        }

        /// <summary>
        /// Creates a new row and creates a readonly copy of it.
        /// Test if no valid index is allowed to be set.
        /// </summary>
        [Test]
        public void SetReadonlyContentWithPartiallyFilledRow()
        {
            // Arrange
            int columnCount = 5;
            Row<object> row = new Row<object>(columnCount, null);

            // Act
            Row<object> readonlyRow = row.AsReadonly();

            // Assert
            for (int index = 0; index < columnCount; index++)
            {
                Assert.Catch(
                typeof(InvalidOperationException),
                () =>
                {
                    readonlyRow[index] = null;
                });
            }
        }

        /// <summary>
        /// Creates a new row and creates a readonly copy.
        /// Tests indices out of range for get and set.
        /// </summary>
        [Test]
        public void AccessInvalidIndicesOnReadonly()
        {
            // Arrange
            int columnCount = 5;
            Row<object> row = new Row<object>(columnCount, null);

            // Act
            Row<object> readonlyRow = row.AsReadonly();

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    readonlyRow[-1] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    object o = readonlyRow[-1];
                });

            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    readonlyRow[columnCount] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    object o = readonlyRow[columnCount];
                });
        }

        /// <summary>
        /// Creates a row with zero columns.
        /// Tests if there are no valid indices.
        /// </summary>
        [Test]
        public void RowWithoutColumns()
        {
            // Arrange
            int columnCount = 0;
            Row<object> row = new Row<object>(columnCount, null);

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    row[-1] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    row[0] = null;
                });
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    row[1] = null;
                });
        }

        /// <summary>
        /// Creates a row with less than zero columns.
        /// Tests if this is invalid.
        /// </summary>
        [Test]
        public void RowWithNegativeNumberColumns()
        {
            // Arrange
            int columnCount = -1;

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    Row<object> row = new Row<object>(columnCount, null);
                });
        }
    }
}
