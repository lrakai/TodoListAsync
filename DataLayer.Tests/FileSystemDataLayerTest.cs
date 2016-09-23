using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSystemDataLayer;
using System.IO;

namespace DataLayer.Tests
{
    [TestClass]
    public class FileSystemDataLayerTest : DataLayerTest
    {
        private static readonly string s_testRelativePath = "testing";

        [TestInitialize]
        public void Setup()
        {
            m_data = new FileSystemDataLayer<Item>(s_testRelativePath);
        }

        [TestCleanup]
        public void Teardown()
        {
            Directory.Delete(s_testRelativePath, true);
        }
    }
}
