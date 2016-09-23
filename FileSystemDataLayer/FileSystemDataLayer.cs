using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Contract;
using System.IO;
using Newtonsoft.Json;

namespace FileSystemDataLayer
{
    /// <summary>
    /// Data layer implementation that uses the local file system for persistence.
    /// </summary>
    /// <typeparam name="T">IIdentifiable item of data.</typeparam>
    public class FileSystemDataLayer<T> : IDataLayer<T> where T : class, IIdentifiable, IDateTimeProvider
    {
        private static readonly string m_fileExtension = ".json";
        /// <summary>
        /// Relative path for where to write files.
        /// </summary>
        private readonly string m_relativePath = "fdl";
        /// <summary>
        /// Relative path of the file persisting the data.
        /// </summary>
        public string RelativePath
        {
            get
            {
                return m_relativePath;
            }
        }

        /// <summary>
        /// Create a FileSystemDataLayer with default RelativePath for writing files.
        /// </summary>
        public FileSystemDataLayer()
        {
        }

        /// <summary>
        /// Create a FileSystemDataLayer with the specified <paramref="relativePath"/> for writing files.
        /// </summary>
        /// <param name="relativePath"></param>
        public FileSystemDataLayer(string relativePath)
        {
            m_relativePath = relativePath;
            Initialize();
        }
        
        /// <summary>
        /// Insert a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to insert.</param>
        /// <returns>Task to await the insert operation.</returns>
        public async Task InsertAsync(T item)
        {
            try
            {
                using (var file = File.Open(ConstructFileName(item), FileMode.CreateNew))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(memoryStream))
                        {
                            var serializer = JsonSerializer.CreateDefault();

                            serializer.Serialize(writer, item);

                            await writer.FlushAsync().ConfigureAwait(false);

                            memoryStream.Seek(0, SeekOrigin.Begin);

                            await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                        }
                    }
                    await file.FlushAsync().ConfigureAwait(false);
                }
                //TODO: include this in critical section
                File.SetCreationTime(ConstructFileName(item), item.DateTime);
            }
            catch (IOException exception)
            {
                var message = String.Format("{0} with Id {1} already inserted.", typeof(T).FullName, item.Id);
                throw new DataLayerAlreadyExistsException(message, exception);
            }
        }

        /// <summary>
        /// Find <typeparamref name="T"/> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparamref name="T"/> to find.</param>
        /// <returns>Task<T> with Result <typeparamref name="T"/> if found, otherwise null.</returns>
        public async Task<T> FindAsync(Guid id)
        {
            try
            {
                return await ReadFile(id);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Find a paged range of <typeparamref name="T"/> ordered with the most recent first.
        /// </summary>
        /// <param name="skip">Number to skip.</param>
        /// <param name="limit">Maximum number to <typeparamref name="T"/> to include, if available.</param>
        /// <returns>Result containing a collection of <typeparamref name="T"/>.</returns>
        public async Task<IEnumerable<T>> FindAsync(int skip, int limit)
        {
            var info = new DirectoryInfo(m_relativePath);
            //TODO: per-file locking
            var files = info.GetFiles()
                .OrderByDescending(p => p.CreationTime)
                .Skip(skip)
                .Take(limit)
                .ToArray();
            var items = new List<T>(files.Count());
            foreach (var file in files)
            {
                var id = Guid.Parse(Path.GetFileNameWithoutExtension(file.Name));
                items.Add(await ReadFile(id));
            }
            return items;
        }

        /// <summary>
        /// Update a <typeparamref name="T">.
        /// </summary>
        /// <param name="item"><typeparam name="T"> to update.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                using (var file = File.Open(ConstructFileName(item), FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(memoryStream))
                        {
                            var serializer = JsonSerializer.CreateDefault();

                            serializer.Serialize(writer, item);

                            await writer.FlushAsync().ConfigureAwait(false);

                            memoryStream.Seek(0, SeekOrigin.Begin);

                            await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                        }
                    }
                    await file.FlushAsync().ConfigureAwait(false);
                }
                //TODO: include this in critical section
                File.SetCreationTime(ConstructFileName(item), item.DateTime);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a <typeparamref name="T"> using it's Id.
        /// </summary>
        /// <param name="id">Id of the <typeparam name="T"> to remove.</param>
        /// <returns>Result true if found and removed, false if not found.</returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            return await Task.Run(() =>
             {
                 var filename = ConstructFileName(id);
                 var exists = File.Exists(filename);
                 if (!exists)
                 {
                     return false;
                 }
                 File.Delete(filename);
                 return true;
             });
        }
        
        private void Initialize()
        {
            Directory.CreateDirectory(m_relativePath);
        }

        private string ConstructFileName(T item)
        {
            return ConstructFileName(item.Id);
        }

        private string ConstructFileName(Guid id)
        { 
            return Path.Combine(m_relativePath, id + m_fileExtension);
        }
        
        private async Task<T> ReadFile(Guid id)
        {
            using (var file = new FileStream(ConstructFileName(id), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[4096];
                int numRead;
                while ((numRead = await file.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Default.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return JsonConvert.DeserializeObject<T>(sb.ToString());
            }
        }
    }
}
