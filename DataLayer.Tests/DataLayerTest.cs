using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer.Contract;
using System.Threading.Tasks;
using System.Linq;

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

            Assert.AreEqual(item, found);
        }

        [TestMethod]
        public async Task TestFindRangeItemAsync()
        {
            var item1 = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MinValue
            };

            var item2 = new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                DateTime = DateTime.MinValue.AddYears(1)
            };

            var item3 = new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                DateTime = DateTime.MinValue.AddYears(2)
            };

            await m_data.InsertAsync(item1);
            await m_data.InsertAsync(item2);
            await m_data.InsertAsync(item3);

            var found = await m_data.FindAsync(1, 10);
            
            var foundList = found.ToList();
            Assert.AreEqual(2, foundList.Count());
            // descending DateTime order
            Assert.AreEqual(item2, foundList[0]);
            Assert.AreEqual(item1, foundList[1]);
        }

        [TestMethod]
        public async Task TestUpdateItemAsync()
        {
            var itemOriginal = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MinValue
            };

            var itemUpdate = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MaxValue
            };

            await m_data.InsertAsync(itemOriginal);

            var updated = await m_data.UpdateAsync(itemUpdate);

            Assert.AreEqual(true, updated);
            var found = await m_data.FindAsync(itemOriginal.Id);
            Assert.AreEqual(itemUpdate, found);
        }

        [TestMethod]
        public async Task TestUpdateNonExistentItemAsync()
        {
            var itemUpdate = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MaxValue
            };
            
            var updated = await m_data.UpdateAsync(itemUpdate);

            Assert.AreEqual(false, updated);
        }

        [TestMethod]
        public async Task TestRemoveItemAsync()
        {
            var item = new Item
            {
                Id = Guid.Empty,
                DateTime = DateTime.MinValue
            };
            
            await m_data.InsertAsync(item);

            var removed = await m_data.RemoveAsync(item.Id);

            Assert.AreEqual(true, removed);
            var found = await m_data.FindAsync(item.Id);
            Assert.AreEqual(null, found);
        }
    }
}
