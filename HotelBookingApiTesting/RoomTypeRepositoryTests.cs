using HotelBookingAPI.Context;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApiTesting
{
    [TestFixture]
    public class RoomTypeRepositoryTests
    {
        //File under test
        private RoomTypeRepository _repository = null!;
        //Mocking DB Context
        private Mock<HotelBookingContext> _mockContext = null!;
        //sample in-memory test data
        private List<RoomType> _roomTypes = null!;

        [SetUp]
        public void Setup()
        {
            // Sample data
            _roomTypes = new List<RoomType>
            {
                new RoomType { RoomTypeId = 1, TypeName = "Deluxe", IsActive = true },
                new RoomType { RoomTypeId = 2, TypeName = "Standard", IsActive = true },
                new RoomType { RoomTypeId = 3, TypeName = "Suite", IsActive = false }
            };

            // Mock DbContext
            _mockContext = new Mock<HotelBookingContext>();
            _mockContext.Setup(c => c.RoomTypes).ReturnsDbSet(_roomTypes);

            // Repository
            _repository = new RoomTypeRepository(_mockContext.Object);
        }

        [Test]
        public async Task IsTypeNameExistsAsync_ReturnsTrue_WhenTypeNameExists()
        {
            var result = await _repository.IsTypeNameExistsAsync("Deluxe");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsTypeNameExistsAsync_ReturnsFalse_WhenTypeNameDoesNotExist()
        {
            var result = await _repository.IsTypeNameExistsAsync("Presidential");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsTypeNameExistsAsync_ReturnsFalse_WhenTypeNameExistsButExcludedIdMatches()
        {
            var result = await _repository.IsTypeNameExistsAsync("Deluxe", excludeId: 1);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetActiveRoomTypesAsync_ReturnsOnlyActiveRoomTypes()
        {
            var result = await _repository.GetActiveRoomTypesAsync();
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEquivalent(new[] { "Deluxe", "Standard" }, result.Select(r => r.TypeName));
        }
    }
}
