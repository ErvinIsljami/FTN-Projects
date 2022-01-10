using NUnit.Framework;
using QueueService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class TestDataModel
    {
        [Test]
        public void TestModel()
        {
            DataModel dataModel = new DataModel();
            Assert.IsNotNull(dataModel);
            Assert.IsNotNull(dataModel.Clients);
            Assert.IsNotNull(dataModel.Items);
            Assert.IsNotNull(dataModel.Positions);
            Assert.IsNotNull(dataModel.Items);
            Assert.AreEqual(dataModel.Id, "");

        }

        [Test]
        public void TestPositions()
        {
            Position position = new Position(1, 2, 3);
            Assert.IsNotNull(position);
            Assert.AreEqual(position.X, 1);
            Assert.AreEqual(position.Y, 2);
            Assert.AreEqual(position.Z, 3);
        }

        [Test]
        public void TestItems()
        {
            Item item = new Item("item", 1, true, 1);
            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, "item");
            Assert.AreEqual(item.Quantity, 1);
            Assert.AreEqual(item.IsActive, true);
            Assert.AreEqual(item.DestructivePower, 1);

        }
    }
}
