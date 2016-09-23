using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Contract;
using System.Collections.Concurrent;

namespace InMemoryDataLayer
{
    /// <summary>
    /// A non-persistent data layer implementation that exists only in memory.
    /// Not an asynchronous implementation.
    /// </summary>
    /// <typeparam name="T">IIdentifiable item of data.</typeparam>
    public class InMemoryDataLayer<T> : IDataLayer<T> where T : class, IIdentifiable, IDateTimeProvider
    {
        private ConcurrentDictionary<Guid, T> m_data = new ConcurrentDictionary<Guid, T>();

        /// <summary>
        /// Insert a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to insert.</param>
        /// <returns>Task to await the insert operation.</returns>
        public Task InsertAsync(T item)
        {
            return Task.Run(() =>
            {
                var addResult = m_data.TryAdd(item.Id, item);
                if (!addResult)
                {
                    var message = String.Format("{0} with Id {1} already inserted.", typeof(T).FullName, item.Id);
                    throw new DataLayerAlreadyExistsException(message);
                }
            });
        }

        /// <summary>
        /// Find <typeparamref name="T"/> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparamref name="T"/> to find.</param>
        /// <returns>Task<T> with Result <typeparamref name="T"/> if found, otherwise null.</returns>
        public Task<T> FindAsync(Guid id)
        {
            return Task.Run(() =>
            {
                T item;
                m_data.TryGetValue(id, out item);
                return item;
            });
        }

        /// <summary>
        /// Find a paged range of <typeparamref name="T"/> ordered with the most recent first.
        /// </summary>
        /// <param name="skip">Number to skip.</param>
        /// <param name="limit">Maximum number to <typeparamref name="T"/> to include, if available.</param>
        /// <returns>Result containing a collection of <typeparamref name="T"/>.</returns>
        public Task<IEnumerable<T>> FindAsync(int skip, int limit)
        {
            return Task.Run(() =>
            {
                return m_data
                    .Values
                    .OrderByDescending(v => v.DateTime)
                    .Skip(skip)
                    .Take(limit);
            });
        }

        /// <summary>
        /// Update a <typeparamref name="T">.
        /// </summary>
        /// <param name="item"><typeparam name="T"> to update.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public Task<bool> UpdateAsync(T item)
        {
            return Task.Run(() =>
            {
                T existingItem;
                var exists = m_data.TryGetValue(item.Id, out existingItem);
                if (!exists)
                {
                    return false;
                }
                m_data.TryUpdate(item.Id, item, existingItem);
                return true;
            });
        }

        /// <summary>
        /// Remove a <typeparamref name="T"> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparam name="T"> to remove.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public Task<bool> RemoveAsync(Guid id)
        {
            return Task.Run(() =>
            {
                T item;
                return m_data.TryRemove(id, out item);
            });
        }
    }
}
