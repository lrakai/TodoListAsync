using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer.Contract;
using System.Threading.Tasks;

namespace DataLayer.Tests
{
    public abstract class DataLayerTest
    {
        protected IDataLayer<Item> m_data;

        [TestMethod]
        [ExpectedException(typeof(DataLayerAlreadyExistsException))]
        public async Task TestInsertSameItemAsync()
        {
            var item = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MinValue
            };

            await m_data.InsertAsync(item);

            await m_data.InsertAsync(item);
        }

        [TestMethod]
        public async Task TestFindItemAsync()
        {
            var item = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MinValue
            };

            await m_data.InsertAsync(item);

            var found = await m_data.FindAsync(item.Id);

            Assert.AreEqual<Item>(item, found);
        }
    }
}
