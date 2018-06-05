// <copyright file="Row.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
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
        /// <exception cref="ArgumentOutOfRangeException">If the column count is less than 0.</exception>
        public Row(int columnCount, T standardReturnObject)
        {
            if (columnCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "No negative column count allowed!");
            }

            this.IsReadonly = false;
            this.StandardReturnObject = standardReturnObject;
            this.Entries = new List<Entry<T>>();
            this.ColumnCount = columnCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Row{T}"/> class.
        /// The new instance is readonly.
        /// </summary>
        /// <param name="columnCount">The number of columns in the row.</param>
        /// <param name="standardReturnObject">The object returned if no specific entry is available.</param>
        /// <param name="readonlyEntries">A correctly ordered list with the readonly entries of the row.</param>
        private Row(int columnCount, T standardReturnObject, List<Entry<T>> readonlyEntries)
        {
            this.IsReadonly = true;
            this.ColumnCount = columnCount;
            this.StandardReturnObject = standardReturnObject;
            this.Entries = readonlyEntries;
        }

        /// <summary>
        /// Gets a value indicating whether the entry is readonly.
        /// </summary>
        public bool IsReadonly { get; }

        /// <summary>
        /// Gets the number of entries in the row that are different from
        /// the standard return object. An entry is only created for values
        /// different from the standard return object. This value corresponds
        /// to the memory usage of the row.
        /// </summary>
        public long EntryCount
        {
                get
                {
                    return this.Entries.Count;
                }
        }

        /// <summary>
        /// Gets the number of columns in the row.
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        /// Gets the object returned if no specific entry is available.
        /// </summary>
        private T StandardReturnObject { get; }

        /// <summary>
        /// Gets the list of entries in the row.
        /// The entries are ordered by their column index that is ascending.
        /// There are no doubled column indices nor ones less than 0 or greater or equal to column count.
        /// </summary>
        private List<Entry<T>> Entries { get; }

        /// <summary>
        /// Gets or sets the content of an entry.
        /// </summary>
        /// <param name="index">The index of the entry.</param>
        /// <returns>The content of the entry.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the index is less than 0 or greater or equal to the column count.</exception>
        /// <exception cref="InvalidOperationException">If the row is readonly.</exception>
        public T this[int index]
        {
            get
            {
                this.TestIndex(index);

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
                this.TestIndex(index);

                if (this.IsReadonly)
                {
                    throw new InvalidOperationException("The row is readonly!");
                }

                Entry<T> entry = this.SearchEntry(index);

                if (entry == null)
                {
                    if (this.IsEqualToStandardReturnObject(value))
                    {
                    }
                    else
                    {
                        // Create a new entry and insert it into its correct position in the list.
                        entry = new Entry<T>(index, value);
                        int insertIndex = this.FindFreePositionForEntryInList(index);
                        this.Entries.Insert(insertIndex, entry);
                    }
                }
                else
                {
                    if (this.IsEqualToStandardReturnObject(value))
                    {
                        this.Entries.Remove(entry);
                    }
                    else
                    {
                        entry.Content = value;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a copy of the Row object that is readonly.
        /// </summary>
        /// <returns>A new Row object, with the column count, standard return object and the entries of the current one, that is readonly.</returns>
        public Row<T> AsReadonly()
        {
            List<Entry<T>> readonlyEntries = new List<Entry<T>>(this.Entries.Count);

            foreach (Entry<T> entry in this.Entries)
            {
                readonlyEntries.Add(entry.AsReadonly());
            }

            return new Row<T>(this.ColumnCount, this.StandardReturnObject, readonlyEntries);
        }

        /// <summary>
        /// Tests if the given object has the same value (if T is a value type)
        /// or reference (if T is not a value type) as the standard return object.
        /// </summary>
        /// <param name="obj">The object for which equality is tested with the standard return object.</param>
        /// <returns>If <paramref name="obj"/> is equal to the standard return object on the basis described above.</returns>
        public bool IsEqualToStandardReturnObject(T obj)
        {
            if (obj == null)
            {
                return this.StandardReturnObject == null;
            }

            if (obj.GetType().IsValueType)
            {
                return obj.Equals(this.StandardReturnObject);
            }

            return object.ReferenceEquals(obj, this.StandardReturnObject);
        }

        /// <summary>
        /// Tests if the index is out of the column range and throws an exception if so.
        /// </summary>
        /// <param name="index">The index to be tested for the valid range.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the index is less than 0 or greater or equal to the column count.</exception>
        private void TestIndex(int index)
        {
            if (index < 0 || index >= this.ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The index must be in the range 0 (included) to " + this.ColumnCount + " (excluded)!");
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
