// <copyright file="Matrix.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This special matrix is used to describe the edges in the graph.
    /// Because of its nature the matrix is rather empty and is implemented to use only a small amount of memory while losing some performance for that.
    /// To achieve that there is a object defined to be returned on default. Remember that no new instance of the standard return object is created!
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public class Matrix<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix{T}"/> class.
        /// The matrix is a square (n x n) matrix.
        /// </summary>
        /// <param name="rowColumnCount">The dimension n of the n x n matrix.</param>
        /// <param name="standardReturnObject">The value returned by every entry where no specific entry is set.</param>
        public Matrix(int rowColumnCount, T standardReturnObject) : this(rowColumnCount, rowColumnCount, standardReturnObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix{T}"/> class.
        /// The matrix is a n x m matrix.
        /// </summary>
        /// <param name="rowCount">The dimension n of the n x m matrix the rows.</param>
        /// <param name="columnCount">The dimension m of the n x m matrix the columns.</param>
        /// <param name="standardReturnObject">The value returned by every entry where no specific entry is set.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the row or column count is negative.</exception>
        public Matrix(int rowCount, int columnCount, T standardReturnObject)
        {
            if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "No negative row count allowed!");
            }

            if (columnCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "No negative column count allowed!");
            }

            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.StandardReturnObject = standardReturnObject;

            this.Rows = new List<Row<T>>(this.RowCount);
            for (int pos = 0; pos < this.RowCount; pos++)
            {
                this.Rows.Add(new Row<T>(this.ColumnCount, this.StandardReturnObject));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix{T}"/> class.
        /// The new instance is readonly.
        /// </summary>
        /// <param name="rowCount">The dimension n of the n x m matrix the rows.</param>
        /// <param name="columnCount">The dimension m of the n x m matrix the columns.</param>
        /// <param name="readonlyRows">A correctly ordered list with the readonly rows.</param>
        private Matrix(int rowCount, int columnCount, List<Row<T>> readonlyRows, T standardReturnObject)
        {
            this.IsReadonly = true;
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.Rows = readonlyRows;
            this.StandardReturnObject = standardReturnObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix{T}"/> class.
        /// The content of the given array is copied into the matrix.
        /// The standard return object is the object that occurs most and
        /// is found first (searching columns and then rows).
        /// </summary>
        /// <param name="multArray">A multidimensional array that is copied into the matrix.</param>
        public Matrix(T[,] multArray)
        {
            this.RowCount = multArray.GetLength(0);
            this.ColumnCount = multArray.GetLength(1);
            this.StandardReturnObject = this.GetObjectFirstOccuringTheMost(multArray);

            this.Rows = new List<Row<T>>(this.RowCount);
            for (int pos = 0; pos < this.RowCount; pos++)
            {
                this.Rows.Add(new Row<T>(this.ColumnCount, this.StandardReturnObject));
            }

            this.CopyMultidimensionalArrayIntoMatrix(multArray);
        }

        /// <summary>
        /// Gets the number of rows in the matrix.
        /// </summary>
        public int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in the matrix.
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        /// Gets the object returned for entries with no specific value.
        /// </summary>
        public T StandardReturnObject { get; }

        /// <summary>
        /// Gets a value indicating whether the entry is readonly.
        /// </summary>
        public bool IsReadonly { get; } = false;

        /// <summary>
        /// Gets the list of the rows in the matrix.
        /// The rows are ordered in ascending order with their index in the list being equal to their row index.
        /// There are no indices lower than 0 or greater or equal to row count.
        /// </summary>
        private List<Row<T>> Rows { get; }

        /// <summary>
        /// Gets a row of the matrix with its index.
        /// </summary>
        /// <param name="index">The index of the row.</param>
        /// <returns>The row that belongs to index.</returns>
        public Row<T> this[int index]
        {
            get
            {
                return this.Rows[index];
            }
        }

        /// <summary>
        /// Gets or sets an entry of the matrix.
        /// </summary>
        /// <param name="row">The index of the row.</param>
        /// <param name="column">The index of the column.</param>
        /// <returns>The entry that belongs to the indices.</returns>
        public T this[int row, int column]
        {
            get
            {
                return this[row][column];
            }

            set
            {
                this[row][column] = value;
            }
        }

        /// <summary>
        /// Creates a copy of the Matrix object that is readonly.
        /// </summary>
        /// <returns>A new Matrix object, with the row count, column count and the rows of the current one, that is readonly.</returns>
        public Matrix<T> AsReadonly()
        {
            List<Row<T>> readonlyRows = new List<Row<T>>(this.RowCount);

            foreach (Row<T> row in this.Rows)
            {
                readonlyRows.Add(row.AsReadonly());
            }

            return new Matrix<T>(this.RowCount, this.ColumnCount, readonlyRows, this.StandardReturnObject);
        }

        /// <summary>
        /// Searches the columns and then the rows for the object occuring the
        /// most and found first.
        /// </summary>
        /// <param name="multArray">The matrix that is searched.</param>
        /// <returns>The object occuring the most and found first.</returns>
        private T GetObjectFirstOccuringTheMost(T[,] multArray)
        {
            List<T> objectsInContentArray = new List<T>();
            List<long> numberOccurancesOfObjectsInContentArray = new List<long>();


            foreach (T entry in multArray)
            {
                if (objectsInContentArray.Contains(entry))
                {
                    int indexOfObject = objectsInContentArray.IndexOf(entry);
                    numberOccurancesOfObjectsInContentArray[indexOfObject] += 1;
                }
                else
                {
                    objectsInContentArray.Add(entry);
                    numberOccurancesOfObjectsInContentArray.Add(1);
                }
            }

            T firstMostOccuringObject = default(T);
            long numberOccurancesOfCurrentObject = -1;

            for (int pos = 0; pos < objectsInContentArray.Count; pos++)
            {
                if (numberOccurancesOfCurrentObject < numberOccurancesOfObjectsInContentArray[pos])
                {
                    firstMostOccuringObject = objectsInContentArray[pos];
                    numberOccurancesOfCurrentObject = numberOccurancesOfObjectsInContentArray[pos];
                }
            }

            return firstMostOccuringObject;
        }

        /// <summary>
        /// Sets the content of this matrix object to the contents of the given
        /// multidimensional array.
        /// </summary>
        /// <param name="multArray">The matrix that should be copied. The dimensions must match matrix.</param>
        private void CopyMultidimensionalArrayIntoMatrix(T[,] multArray)
        {
            for (int row = 0; row < this.RowCount; row++)
            {
                for (int column = 0; column < this.ColumnCount; column++)
                {
                    this[row, column] = multArray[row, column];
                }
            }
        }
    }
}
