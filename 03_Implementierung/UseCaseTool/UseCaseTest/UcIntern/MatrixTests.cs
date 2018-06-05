// <copyright file="MatrixTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Matrix{T}"/> class.
    /// </summary>
    [TestFixture]
    public class MatrixTests
    {
        /// <summary>
        /// Creates a n x m matrix and checks the dimensions.
        /// </summary>
        [Test]
        public void CreateRecatangularMatrix()
        {
            // Arrange
            int rowCount = 4,
                columnCount = 2;
            Matrix<object> matrix = new Matrix<object>(rowCount, columnCount, null);

            // Act

            // Assert
            Assert.AreEqual(rowCount, matrix.RowCount);
            Assert.AreEqual(columnCount, matrix.ColumnCount);
        }

        /// <summary>
        /// Creates a n x n matrix and checks the dimensions.
        /// </summary>
        [Test]
        public void CreateSquareMatrix()
        {
            // Arrange
            int rowColumnCount = 3;
            Matrix<object> matrix = new Matrix<object>(rowColumnCount, null);

            // Act

            // Assert
            Assert.AreEqual(rowColumnCount, matrix.RowCount);
            Assert.AreEqual(rowColumnCount, matrix.ColumnCount);
        }

        /// <summary>
        /// Tries to creates a matrix with negative row count.
        /// </summary>
        [Test]
        public void TryCreateMatrixWithNegativeRowCount()
        {
            // Arrange

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    Matrix<object> matrix = new Matrix<object>(-1, 0, null);
                });
        }

        /// <summary>
        /// Creates a matrix with negative column count.
        /// </summary>
        [Test]
        public void TryCreateMatrixWithNegativeColumnCount()
        {
            // Arrange

            // Act

            // Assert
            Assert.Catch(
                typeof(ArgumentOutOfRangeException),
                () =>
                {
                    Matrix<object> matrix = new Matrix<object>(0, -1, null);
                });
        }

        /// <summary>
        /// Creates a n x m matrix and fills it with some entries different from
        /// the standard return value.
        /// Test every index for the correct return value.
        /// </summary>
        [Test]
        public void SetGetMatrix()
        {
            // Arrange
            string standardString = "Standard string";
            string[][] testMatrix = new string[][]
            {
                new string[] { "1", standardString, "3" },
                new string[] { "4", "5", standardString }
            };
            Matrix<object> matrix = new Matrix<object>(testMatrix.Length, testMatrix[0].Length, standardString);

            // Act
            for (int row = 0; row < testMatrix.Length; row++)
            {
                for (int column = 0; column < testMatrix[0].Length; column++)
                {
                    if (!testMatrix[row][column].Equals(standardString))
                    {
                        matrix[row, column] = testMatrix[row][column];
                    }
                }
            }

            // Assert
            for (int row = 0; row < testMatrix.Length; row++)
            {
                for (int column = 0; column < testMatrix[0].Length; column++)
                {
                    Assert.AreSame(testMatrix[row][column], matrix[row, column]);
                }
            }
        }

        /// <summary>
        /// Creates a n x m matrix and tries to accesses invalid indices
        /// </summary>
        [Test]
        public void AccessInvalidIndices()
        {
            // Arrange
            int rowCount = 8,
                columnCount = 2;
            Matrix<object> matrix = new Matrix<object>(rowCount, columnCount, null);

            // Act

            // Assert
            for (int row = -1; row <= rowCount; row++)
            {
                for (int column = -1; column <= columnCount; column++)
                {
                    if (
                        row < 0 || row >= rowCount
                        || column < 0 || column >= columnCount)
                    {
                        Assert.Catch(
                        typeof(ArgumentOutOfRangeException),
                        () =>
                        {
                            matrix[row, column] = null;
                        });
                        Assert.Catch(
                        typeof(ArgumentOutOfRangeException),
                        () =>
                        {
                            object o = matrix[row, column];
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new matrix and creates a readonly copy of it.
        /// Test if the IsReadonly property is set correctly.
        /// </summary>
        [Test]
        public void TestIsReadonly()
        {
            // Arrange
            int rowCount = 7,
                columnCount = 4;
            Matrix<object> matrix = new Matrix<object>(rowCount, columnCount, null);

            // Act
            Matrix<object> readonlyMatrix = matrix.AsReadonly();

            // Assert
            Assert.AreEqual(false, matrix.IsReadonly);
            Assert.AreEqual(true, readonlyMatrix.IsReadonly);
        }

        /// <summary>
        /// Creates a n x m matrix, fills it with some entries different from
        /// the standard return value and creates a readonly copy.
        /// Test every index of the copy for the correct return value.
        /// </summary>
        [Test]
        public void GetReadonlyMatrix()
        {
            // Arrange
            string standardString = "Standard string";
            string[][] testMatrix = new string[][]
            {
                new string[] { "1", standardString, "3" },
                new string[] { "4", "5", standardString }
            };
            Matrix<object> matrix = new Matrix<object>(testMatrix.Length, testMatrix[0].Length, standardString);

            for (int row = 0; row < testMatrix.Length; row++)
            {
                for (int column = 0; column < testMatrix[0].Length; column++)
                {
                    if (!testMatrix[row][column].Equals(standardString))
                    {
                        matrix[row, column] = testMatrix[row][column];
                    }
                }
            }

            // Act
            Matrix<object> readonlyMatrix = matrix.AsReadonly();

            // Assert
            for (int row = 0; row < testMatrix.Length; row++)
            {
                for (int column = 0; column < testMatrix[0].Length; column++)
                {
                    Assert.AreSame(testMatrix[row][column], matrix[row, column]);
                }
            }
        }

        /// <summary>
        /// Creates a n x m matrix, fills it with some entries different from
        /// the standard return value and creates a readonly copy.
        /// Test that no valid index of the copy can be set.
        /// </summary>
        [Test]
        public void SetReadonlyMatrix()
        {
            // Arrange
            int rowCount = 5,
                columnCount = 7;
            Matrix<object> matrix = new Matrix<object>(rowCount, columnCount, null);

            // Act
            Matrix<object> readonlyMatrix = matrix.AsReadonly();

            // Assert
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    Assert.Catch(
                        typeof(InvalidOperationException),
                        () =>
                        {
                            readonlyMatrix[row, column] = null;
                        });
                }
            }
        }

        /// <summary>
        /// Creates a n x m matrix, creates a readonly copy
        /// and tries to accesses invalid indices.
        /// </summary>
        [Test]
        public void AccessInvalidIndicesOnReadonlyMatrix()
        {
            // Arrange
            int rowCount = 6,
                columnCount = 4;
            Matrix<object> matrix = new Matrix<object>(rowCount, columnCount, null);

            // Act
            Matrix<object> readonlyMatrix = matrix.AsReadonly();

            // Assert
            for (int row = -1; row <= rowCount; row++)
            {
                for (int column = -1; column <= columnCount; column++)
                {
                    if (
                        row < 0 || row >= rowCount
                        || column < 0 || column >= columnCount)
                    {
                        Assert.Catch(
                        typeof(ArgumentOutOfRangeException),
                        () =>
                        {
                            readonlyMatrix[row, column] = null;
                        });
                        Assert.Catch(
                        typeof(ArgumentOutOfRangeException),
                        () =>
                        {
                            object o = readonlyMatrix[row, column];
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Creates a matrix from an multidimensional array and creates a new matrix from it.
        /// </summary>
        [Test]
        public void CreateMatrixFromMultidimensionalArray()
        {
            // Arrange
            bool[,] expected = new bool[,]
            {
                { true, false, false },
                { false, true, false },
                { false, false, true },
                { false, false, true }
            };

            // Act
            Matrix<bool> matrix = new Matrix<bool>(expected);

            // Assert
            // Test content
            for (int row = 0; row < expected.GetLength(0); row++)
            {
                for (int column = 0; column < expected.GetLength(1); column++)
                {
                    Assert.AreEqual(expected[row, column], matrix[row, column]);
                }
            }

            // Test standard return object
            Assert.AreEqual(false, matrix.StandardReturnObject);
        }

        /// <summary>
        /// Tests the memory saving behaviour of the matrix.
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
        /// Tests the memory saving behaviour of the matrix.
        /// For that purpose the matrix is slowly filled and emptied again with a object different from the standard return object.
        /// While doing that the entry count is monitored.
        /// Make sure the standard return object and the fill object differ.
        /// </summary>
        /// <typeparam name="T">The type of the matrix that is to be tested.</typeparam>
        /// <param name="standardReturnObject">The object set as standard return object in the matrix.</param>
        /// <param name="fillObject">The object with which the matrix is filled.</param>
        public void TestMemorySavingBehaviour<T>(T standardReturnObject, T fillObject)
        {
            Assert.AreNotEqual(standardReturnObject, fillObject);

            // Arrange
            int rowCount = 2,
                columnCount = 2;
            Matrix<T> matrix = new Matrix<T>(rowCount, columnCount, standardReturnObject);

            // Fill
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    matrix[row, column] = fillObject;
                    Assert.AreEqual((row * matrix.ColumnCount) + column + 1, matrix.EntryCount);
                }
            }

            // Empty
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    matrix[row, column] = standardReturnObject;
                    Assert.AreEqual((matrix.RowCount * matrix.ColumnCount) - ((row * matrix.ColumnCount) + column + 1), matrix.EntryCount);
                }
            }
        }
    }
}
