using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSystemDataLayer;

namespace DataLayer.Tests
{
    [TestClass]
    public class FileSystemDataLayerTest : DataLayerTest
    {
        [TestInitialize]
        public void Setup()
        {
            m_data = new FileSystemDataLayer<Item>();
        }
    }
}
