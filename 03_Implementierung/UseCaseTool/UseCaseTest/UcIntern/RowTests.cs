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

        /// <summary>
        /// Tests the memory saving behaviour of the row.
        /// For that purpose the matrix is slowly filled and emptied again with a object different from the standard return object.
        /// While doing that the entry count is monitored.
        /// The test is executed for different types of generic arguments that are: ValueTypes, Nullable ValueTypes and ReferenceTypes
        /// </summary>
        [Test]
        public void TestMemorySavingBehaviour()
        {
            this.TestMemorySavingBehaviour<bool>(false, true);
            this.TestMemorySavingBehaviour<bool?>(null, true);
            this.TestMemorySavingBehaviour<object>(new object(), new object());
        }

        /// <summary>
        /// Tests the memory saving behaviour of the row.
        /// For that purpose the row is slowly filled and emptied again with a object different from the standard return object.
        /// While doing that the entry count is monitored.
        /// Make sure the standard return object and the fill object differ.
        /// </summary>
        /// <typeparam name="T">The type of the row that is to be tested.</typeparam>
        /// <param name="standardReturnObject">The object set as standard return object in the row.</param>
        /// <param name="fillObject">The object with which the row is filled.</param>
        public void TestMemorySavingBehaviour<T>(T standardReturnObject, T fillObject)
        {
            Assert.AreNotEqual(standardReturnObject, fillObject);

            // Arrange
            int columnCount = 2;
            Row<T> row = new Row<T>(columnCount, standardReturnObject);

            // Fill
            for (int column = 0; column < row.ColumnCount; column++)
            {
                row[column] = fillObject;
                Assert.AreEqual(column + 1, row.EntryCount);
            }

            // Empty
            for (int column = 0; column < row.ColumnCount; column++)
            {
                row[column] = standardReturnObject;
                Assert.AreEqual(row.ColumnCount - (column + 1), row.EntryCount);
            }
        }

        /// <summary>
        /// Sets the value in a row and changes it.
        /// </summary>
        [Test]
        public void ReassignEntryToNewValue()
        {
            // Arrange
            int oldValue = 2,
                newValue = 51;
            Row<int> row = new Row<int>(2, 0);

            // Act
            row[0] = oldValue;
            row[0] = newValue;

            // Assert
            Assert.AreEqual(newValue, row[0]);
        }

        /// <summary>
        /// Checks if the standard return object is identified correctly when setting it to a entry.
        /// For the scenario imagine that you have an class instance whose Equals method returns true if executed with the standard return object.
        /// This object is assigned to an entry. Now you get the element from the row and change it. For the test to succeed now the
        /// standard return object is not allowed to have changed.
        /// </summary>
        [Test]
        public void IdentifyStandardReturnObjectOnSet()
        {
            // Arrange
            Test t = new Test() { i = 0 },
                srt = new Test() { i = 0 };
            Row<Test> row = new Row<Test>(2, srt);

            // Act
            row[0] = t;
            row[0].i++;

            // Assert
            Assert.AreEqual(0, srt.i);
        }

        internal class Test
        {
            public int i;

            public override bool Equals(object obj)
            {
                return obj is Test && i == ((Test)obj).i;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }
    }
}
