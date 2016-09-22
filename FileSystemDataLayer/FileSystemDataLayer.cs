using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Contract;

namespace FileSystemDataLayer
{
    /// <summary>
    /// Data layer implementation that uses the local file system for persistence.
    /// </summary>
    /// <typeparam name="T">IIdentifiable item of data.</typeparam>
    public class FileSystemDataLayer<T> : IDataLayer<T> where T : IIdentifiable, IDateTimeProvider
    {
        /// <summary>
        /// Insert a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to insert.</param>
        /// <returns>Task to await the insert operation.</returns>
        public Task InsertAsync(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find <typeparamref name="T"/> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparamref name="T"/> to find.</param>
        /// <returns>Task<T> with Result <typeparamref name="T"/> if found, otherwise null.</returns>
        public Task<T> FindAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find a paged range of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="skip">Number to skip.</param>
        /// <param name="limit">Maximum number to <typeparamref name="T"/> to include, if available.</param>
        /// <returns>Result containing a collection of <typeparamref name="T"/>.</returns>
        public Task<IEnumerable<T>> FindAsync(uint skip, uint limit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update a <typeparamref name="T">.
        /// </summary>
        /// <param name="item"><typeparam name="T"> to update.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public Task<bool> UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a <typeparamref name="T"> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparam name="T"> to remove.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public Task<bool> RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
