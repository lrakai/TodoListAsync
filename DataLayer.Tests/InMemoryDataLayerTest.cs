using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemoryDataLayer;

namespace DataLayer.Tests
{
    [TestClass]
    public class InMemoryDataLayerTest : DataLayerTest
    {
        [TestInitialize]
        public void Setup()
        {
            m_data = new InMemoryDataLayer<Item>();
        }
    }
}
