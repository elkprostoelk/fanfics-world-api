using AutoMapper;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Services;
using FanficsWorld.UnitTests.TestData;
using FanficsWorld.WebAPI.Configurations;
using Microsoft.Extensions.Configuration;

namespace FanficsWorld.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;
    
    public UserServiceTests()
    {
        var mapper = new MapperConfiguration(expr=>
            expr.AddProfile<AutoMapperProfile>())
            .CreateMapper();
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object, mapper, configuration);
    }

    [Theory]
    [ClassData(typeof(UserServiceTestsTestData))]
    public async Task GetSimpleUsersChunkAsync_UserIdNotSpecified_ReturnsAll(string userId, long itemsCount, List<User> users)
    {
        // Arrange
        
        _userRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<string>()))
            .ReturnsAsync(users);
        _userRepositoryMock.Setup(r => r.CountAsync(It.IsAny<string>()))
            .ReturnsAsync(itemsCount);
        
        // Act

        var result = await _userService.GetSimpleUsersAsync(userId);

        // Assert
        
        Assert.NotEmpty(result);
        Assert.Equal(users.Count, result.Count);
    }
}