using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository repository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> RegisterUserAsync(RegisterUserDTO registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);
        try
        {
            var result = await _repository.RegisterUserAsync(user, registerUserDto.Password);
            return result.Succeeded;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return false;
        }
    }
}