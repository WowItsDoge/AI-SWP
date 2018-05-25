// <copyright file="Entry.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;

    /// <summary>
    /// The entry of a matrix. It holds the content and its column index.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public class Entry<T>
    {
        /// <summary>
        /// The content of the entry.
        /// </summary>
        private T content;

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
        public T Content
        {
            get
            {
                return this.content;
            }

            set
            {
                if (this.IsReadonly)
                {
                    throw new InvalidOperationException("The entry is readonly!");
                }

                this.content = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entry is readonly.
        /// </summary>
        public bool IsReadonly { get; private set; }

        /// <summary>
        /// Creates a copy of the Entry object that is readonly.
        /// </summary>
        /// <returns>A new Entry object, with the column index and the content of the current one, that is readonly.</returns>
        public Entry<T> AsReadonly()
        {
            Entry<T> readonlyEntry = new Entry<T>(this.ColumnIndex, this.Content);
            readonlyEntry.IsReadonly = false;

            return readonlyEntry;
        }
    }
}
