using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Repositories.Interfaces;
using HotelBookingAPI.Services;
using Moq;

namespace HotelBookingApiTesting
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock = null!;
        private UserService _userService = null!;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Test]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("1234");
            var user = new User { UserId = "U001", Email = "test@gmail.com", PasswordHash = hashedPassword };

            _userRepoMock.Setup(r => r.GetByEmailAsync("test@gmail.com"))
                         .ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateAsync("test@gmail.com", "1234");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test@gmail.com", result!.Email);
        }

        [Test]
        public async Task AuthenticateAsync_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByEmailAsync("invalid@gmail.com"))
                         .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.AuthenticateAsync("invalid@gmail.com", "1234");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AuthenticateAsync_WithWrongPassword_ReturnsNull()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("1234");
            var user = new User { UserId = "U001", Email = "test@gmail.com", PasswordHash = hashedPassword };

            _userRepoMock.Setup(r => r.GetByEmailAsync("test@gmail.com"))
                         .ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateAsync("test@gmail.com", "wrongpassword");

            // Assert
            Assert.IsNull(result);
        }
    }
}