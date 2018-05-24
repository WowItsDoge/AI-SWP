// <copyright file="Matrix.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
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
        public Matrix(int rowCount, int columnCount, T standardReturnObject)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;

            this.Rows = new List<Row<T>>(this.RowCount);
            for (int pos = 0; pos < this.RowCount; pos++)
            {
                this.Rows[pos] = new Row<T>(this.ColumnCount, standardReturnObject);
            }
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
        /// Gets the list of the rows in the matrix.
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
    }
}
