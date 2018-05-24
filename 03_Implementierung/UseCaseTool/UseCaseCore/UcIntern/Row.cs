// <copyright file="Row.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System.Collections.Generic;

    /// <summary>
    /// The row of a matrix with its entries.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public class Row<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Row{T}"/> class.
        /// </summary>
        /// <param name="columnCount">The number of columns in the row.</param>
        /// <param name="standardReturnObject">The object returned if no specific entry is available.</param>
        public Row(int columnCount, T standardReturnObject)
        {
            this.StandardReturnObject = standardReturnObject;
            this.Entries = new List<Entry<T>>();
            this.ColumnCount = columnCount;
        }

        /// <summary>
        /// Gets the object returned if no specific entry is available.
        /// </summary>
        private T StandardReturnObject { get; }

        /// <summary>
        /// Gets the list of entries in the row.
        /// </summary>
        private List<Entry<T>> Entries { get; }

        /// <summary>
        /// Gets the number of columns in the row.
        /// </summary>
        private int ColumnCount { get; }

        /// <summary>
        /// Gets or sets the content of an entry.
        /// </summary>
        /// <param name="index">The index of the entry.</param>
        /// <returns>The content of the entry.</returns>
        public T this[int index]
        {
            get
            {
                Entry<T> entry = this.SearchEntry(index);

                if (entry != null)
                {
                    return entry.Content;
                }
                else
                {
                    return this.StandardReturnObject;
                }
            }

            set
            {
                Entry<T> entry = this.SearchEntry(index);

                if (entry != null)
                {
                    entry.Content = value;
                }
                else
                {
                    // Create a new entry and insert it into its correct position in the list.
                    entry = new Entry<T>(index, value);
                    int insertIndex = this.FindFreePositionForEntryInList(index);
                    this.Entries.Insert(insertIndex, entry);
                }
            }
        }

        /// <summary>
        /// Searches for an entry for a specific index.
        /// </summary>
        /// <param name="index">The index to search for.</param>
        /// <returns>The corresponding entry object or null if the index is not used.</returns>
        private Entry<T> SearchEntry(int index)
        {
            Entry<T> returnEntry = null;

            // Search for the first index that exceeds or is equal to the searched index.
            // If the index is matched the selected entry contains valid content that can be returned.
            // Otherwise there is no entry object for the index so the standard return object can be returned.
            // In both cases the search can be aborted.
            foreach (Entry<T> entry in this.Entries)
            {
                if (entry.ColumnIndex >= index)
                {
                    if (entry.ColumnIndex == index)
                    {
                        returnEntry = entry;
                    }

                    break;
                }
            }

            return returnEntry;
        }

        /// <summary>
        /// Finds the index index where to insert a new entry with an column index that not yet exists in the list.
        /// </summary>
        /// <param name="index">The new column index.</param>
        /// <returns>The index where to insert the new entry and -1 if the index is column index is already in the list.</returns>
        private int FindFreePositionForEntryInList(int index)
        {
            Entry<T> entry;
            int pos;

            for (pos = 0; pos < this.Entries.Count; pos++)
            {
                entry = this.Entries[pos];
                    
                if (entry.ColumnIndex >= index)
                {
                    if (entry.ColumnIndex == index)
                    {
                        return -1;
                    }

                    break;
                }
            }

            return pos;
        }
    }
}
