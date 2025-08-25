using HotelBookingAPI.Context;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApiTesting
{
    [TestFixture]
    public class RoomRepositoryTests
    {
        private Mock<HotelBookingContext> _mockContext = null!;
        private RoomRepository _repository = null!;
        private List<Room> _rooms = null!;
        private List<RoomType> _roomTypes = null!;

        [SetUp]
        public void Setup()
        {
            // Sample RoomTypes
            _roomTypes = new List<RoomType>
            {
                new RoomType { RoomTypeId = 1, TypeName = "Deluxe", IsActive = true },
                new RoomType { RoomTypeId = 2, TypeName = "Standard", IsActive = true }
            };

            // Sample Rooms
            _rooms = new List<Room>
            {
                new Room { RoomId = 1, RoomNumber = "101", IsAvailable = true, RoomTypeId = 1, RoomType = _roomTypes[0] },
                new Room { RoomId = 2, RoomNumber = "102", IsAvailable = false, RoomTypeId = 1, RoomType = _roomTypes[0] },
                new Room { RoomId = 3, RoomNumber = "201", IsAvailable = true, RoomTypeId = 2, RoomType = _roomTypes[1] }
            };

            // Mock DbContext
            _mockContext = new Mock<HotelBookingContext>();
            _mockContext.Setup(c => c.Rooms).ReturnsDbSet(_rooms);
            _mockContext.Setup(c => c.RoomTypes).ReturnsDbSet(_roomTypes);

            // Repository
            _repository = new RoomRepository(_mockContext.Object);
        }

        [Test]
        public async Task GetAvailableRoomsAsync_ReturnsOnlyAvailableRooms()
        {
            var result = await _repository.GetAvailableRoomsAsync();
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEquivalent(new[] { "101", "201" }, result.Select(r => r.RoomNumber));
        }

        [Test]
        public async Task GetRoomsByTypeAsync_ReturnsRoomsForGivenType()
        {
            var result = await _repository.GetRoomsByTypeAsync(1);
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEquivalent(new[] { "101", "102" }, result.Select(r => r.RoomNumber));
        }

        [Test]
        public async Task IsRoomNumberExistsAsync_ReturnsTrue_WhenRoomNumberExists()
        {
            var result = await _repository.IsRoomNumberExistsAsync("101");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsRoomNumberExistsAsync_ReturnsFalse_WhenRoomNumberDoesNotExist()
        {
            var result = await _repository.IsRoomNumberExistsAsync("999");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsRoomNumberExistsAsync_ReturnsFalse_WhenRoomNumberExistsButExcludedIdMatches()
        {
            var result = await _repository.IsRoomNumberExistsAsync("101", excludeId: 1);
            Assert.IsFalse(result);
        }
    }
}
