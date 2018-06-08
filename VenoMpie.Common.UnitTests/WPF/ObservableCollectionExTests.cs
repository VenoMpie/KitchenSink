using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.WPF.Extensions.Collections;
using System.Collections.Generic;

namespace VenoMpie.Common.UnitTests.WPF
{
    [TestClass]
    public class ObservableCollectionExTests
    {
        [TestMethod]
        public void ObservableCollectionEx_TestAddRange()
        {
            ObservableCollectionEx<int> collection = new ObservableCollectionEx<int>();
            List<int> list = new List<int>();
            for (var i = 1; i <= 300000; i++)
            {
                list.Add(i);
            }
            collection.AddRange(list);

            Assert.AreEqual(300000, collection.Count);
        }
        [TestMethod]
        public void ObservableCollectionEx_TestInsertRange()
        {
            ObservableCollectionEx<int> collection = new ObservableCollectionEx<int>();
            List<int> list = new List<int>();
            for (var i = 200001; i <= 300000; i++)
            {
                list.Add(i);
            }
            collection.AddRange(list);

            Assert.AreEqual(100000, collection.Count);
            Assert.AreEqual(300000, collection[collection.Count - 1]);

            list = new List<int>();
            for (var i = 1; i <= 100000; i++)
            {
                list.Add(i);
            }
            collection.InsertRange(list);

            Assert.AreEqual(200000, collection.Count);
            Assert.AreEqual(1, collection[0]);
            Assert.AreEqual(100000, collection[99999]);
            Assert.AreEqual(300000, collection[collection.Count - 1]);

            list = new List<int>();
            for (var i = 100001; i <= 200000; i++)
            {
                list.Add(i);
            }
            collection.InsertRange(100000, list);

            Assert.AreEqual(300000, collection.Count);
            Assert.AreEqual(1, collection[0]);
            Assert.AreEqual(100000, collection[99999]);
            Assert.AreEqual(100001, collection[100000]);
            Assert.AreEqual(200000, collection[199999]);
            Assert.AreEqual(200001, collection[200000]);
            Assert.AreEqual(300000, collection[collection.Count - 1]);
        }
        [TestMethod]
        public void ObservableCollectionEx_TestRemoveRange()
        {
            ObservableCollectionEx<int> collection = new ObservableCollectionEx<int>();
            List<int> list = new List<int>();
            for (var i = 1; i <= 300000; i++)
            {
                list.Add(i);
            }
            collection.AddRange(list);

            Assert.AreEqual(300000, collection.Count);

            collection.RemoveRange(100000);

            Assert.AreEqual(200000, collection.Count);
            Assert.AreEqual(100001, collection[0]);
            Assert.AreEqual(100050, collection[49]);
            Assert.AreEqual(100051, collection[50]);
            Assert.AreEqual(100100, collection[99]);
            Assert.AreEqual(300000, collection[collection.Count - 1]);

            collection.RemoveRange(50, 50);

            Assert.AreEqual(199950, collection.Count);
            Assert.AreEqual(100050, collection[49]);
            Assert.AreEqual(100101, collection[50]);
            Assert.AreEqual(100150, collection[99]);
            Assert.AreEqual(300000, collection[collection.Count - 1]);
        }
    }
}
