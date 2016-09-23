using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Contract
{
    /// <summary>
    /// A simple CRUD-like interface.
    /// </summary>
    /// <typeparam name="T">Identifiable data item that provides a DateTime.</typeparam>
    public interface IDataLayer<T> where T : class, IIdentifiable, IDateTimeProvider
    {
        /// <summary>
        /// Insert a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to insert.</param>
        /// <returns>Task to await the insert operation.</returns>
        Task InsertAsync(T item);

        /// <summary>
        /// Find <typeparamref name="T"/> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparamref name="T"/> to find.</param>
        /// <returns>Task<T> with Result <typeparamref name="T"/> if found, otherwise null.</returns>
        Task<T> FindAsync(Guid id);

        /// <summary>
        /// Find a paged range of <typeparamref name="T"/> ordered with the most recent first.
        /// </summary>
        /// <param name="skip">Number to skip.</param>
        /// <param name="limit">Maximum number to <typeparamref name="T"/> to include, if available.</param>
        /// <returns>Result containing a collection of <typeparamref name="T"/>.</returns>
        Task<IEnumerable<T>> FindAsync(int skip, int limit);

        /// <summary>
        /// Update a <typeparamref name="T">.
        /// </summary>
        /// <param name="item"><typeparam name="T"> to update.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        Task<bool> UpdateAsync(T item);

        /// <summary>
        /// Remove a <typeparamref name="T"> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparam name="T"> to remove.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        Task<bool> RemoveAsync(Guid id);
    }
}
