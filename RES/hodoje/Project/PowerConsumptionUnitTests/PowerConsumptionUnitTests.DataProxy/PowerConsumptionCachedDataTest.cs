using DataAccess;
using DataProxy;
using Entities.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerConsumptionUnitTests.PowerConsumptionUnitTests.DataProxy
{
    [TestFixture]
    class PowerConsumptionCachedDataTest
    {
        private IEnumerable<PowerConsumptionData> data;

        [SetUp]
        public void SetUp()
        {
            data = new List<PowerConsumptionData>()
            {
                new PowerConsumptionData()
                {
                    Id = 0,
                    Timestamp = DateTime.Now,
                    Consumption = 0,
                    GeoArea = null,
                    GeoAreaId = "SRB"
                },
                new PowerConsumptionData()
                {
                    Id = 1,
                    Timestamp = DateTime.Now,
                    Consumption = 0,
                    GeoArea = null,
                    GeoAreaId = "SRB"
                },
                new PowerConsumptionData()
                {
                    Id = 2,
                    Timestamp = DateTime.Now,
                    Consumption = 0,
                    GeoArea = null,
                    GeoAreaId = "SRB"
                },
                new PowerConsumptionData()
                {
                    Id = 3,
                    Timestamp = DateTime.Now,
                    Consumption = 0,
                    GeoArea = null,
                    GeoAreaId = "SRB"
                }
            }.AsEnumerable();
        }

        [Test]
        public void PowerConsumptionCachedDataGoodConstructorParameters()
        {
            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(),
                                                        new UnitOfWork(new DatabaseContext()));

            Assert.IsNotNull(pccd);
        }

        [Test]
        public void PowerConsumptionCachedDataNullConstructorParameters()
        {
            var pccd = new PowerConsumptionCachedData(null as ICacheManager<PowerConsumptionData>, null as IUnitOfWork);

            Assert.IsNotNull(pccd);
        }

        [Test]
        public void PowerConsumptionCachedDataNoDateEnteredGetAll()
        {
            var inputDate = new InputDate()
            {
                From = DateTime.MinValue,
                To = DateTime.MinValue
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(x => x.PowerConsumptionDataRepository.GetAll()).Returns(data);

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(),
                mockUnitOfWork.Object);
            var result = pccd.Get(inputDate);

            //CollectionAssert.AreEqual(data, result);
            foreach (PowerConsumptionData d in result)
            {
                Assert.AreEqual(d, data.FirstOrDefault(x => x.Id == d.Id));
            }
        }

        [Test]
        public void PowerConsumptionCachedDataNoFromDateEnteredFind()
        {
            var inputDate = new InputDate()
            {
                From = new DateTime(1, 1, 1, 0, 0, 0, 0),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second + 1)
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(m => m.PowerConsumptionDataRepository.Find(It.IsAny<Expression<Func<PowerConsumptionData, bool>>>())).Returns(() => data.Where(x => x.Timestamp <= inputDate.To).ToList());

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(),
                mockUnitOfWork.Object);

            var result = pccd.Get(inputDate);

            foreach (PowerConsumptionData d in result)
            {
                Assert.LessOrEqual(d.Timestamp, inputDate.To);
            }
        }

        [Test]
        public void PowerConsumptionCachedDataNoToDateEnteredFind()
        {
            var inputDate = new InputDate()
            {
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                To = new DateTime(1, 1, 1, 0, 0, 0, 0)
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(m => m.PowerConsumptionDataRepository.Find(It.IsAny<Expression<Func<PowerConsumptionData, bool>>>())).Returns(() => data.Where(x => x.Timestamp >= inputDate.From).ToList());

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(),
                mockUnitOfWork.Object);

            var result = pccd.Get(inputDate);

            foreach (PowerConsumptionData d in result)
            {
                Assert.GreaterOrEqual(d.Timestamp, inputDate.To);
            }
        }

        [Test]
        public void PowerConsumptionCachedDataGoodParametersEnteredFind()
        {
            var inputDate = new InputDate()
            {
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour - 1, DateTime.Now.Minute, DateTime.Now.Second),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(m => m.PowerConsumptionDataRepository.Find(It.IsAny<Expression<Func<PowerConsumptionData, bool>>>())).Returns(() => data.Where(x => x.Timestamp >= inputDate.From && x.Timestamp <= inputDate.To).ToList());

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(), mockUnitOfWork.Object);

            var result = pccd.Get(inputDate);

            foreach (PowerConsumptionData d in result)
            {
                Assert.GreaterOrEqual(d.Timestamp, inputDate.From);
                Assert.LessOrEqual(d.Timestamp, inputDate.To);
            }
        }

        [Test]
        public void PowerConsumptionCachedDataSameDateTwiceTestCache()
        {
            var inputDate = new InputDate()
            {
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour - 1, DateTime.Now.Minute, DateTime.Now.Second),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(m => m.PowerConsumptionDataRepository.Find(It.IsAny<Expression<Func<PowerConsumptionData, bool>>>())).Returns(() => data.Where(x => x.Timestamp >= inputDate.From && x.Timestamp <= inputDate.To).ToList());

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(), mockUnitOfWork.Object);

            var result = pccd.Get(inputDate);

            foreach (PowerConsumptionData d in result)
            {
                Assert.GreaterOrEqual(d.Timestamp, inputDate.From);
                Assert.LessOrEqual(d.Timestamp, inputDate.To);
            }

            var cachedResult = pccd.Get(inputDate);
            Assert.AreEqual(cachedResult, result);
        }

        [Test]
        public void PowerConsumptionCacheDataDateOutOfBounds()
        {
            var inputDate = new InputDate()
            {
                From = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                To = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
            };

            var mockUnitOfWork = new Mock<UnitOfWork>();
            mockUnitOfWork.Setup(m => m.PowerConsumptionDataRepository.Find(It.IsAny<Expression<Func<PowerConsumptionData, bool>>>())).Returns(() => new List<PowerConsumptionData>());

            var pccd = new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(), mockUnitOfWork.Object);

            var result = pccd.Get(inputDate);

            Assert.IsEmpty(result);
        }
    }
}
