﻿// <copyright file="Entry.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// The entry of a matrix. It holds the content and its column index.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public class Entry<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entry{T}"/> class.
        /// </summary>
        /// <param name="columnIndex">The index of the column of the entry.</param>
        /// <param name="content">The content of the entry.</param>
        public Entry(int columnIndex, T content)
        {
            this.ColumnIndex = columnIndex;
            this.Content = content;
        }

        /// <summary>
        /// Gets the index of the column of the entry.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets or sets the content of the entry.
        /// </summary>
        public T Content { get; set; }
    }
}