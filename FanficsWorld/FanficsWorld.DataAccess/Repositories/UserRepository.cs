using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FanficsWorld.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FanficsDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserRepository> _logger;
    private readonly IMemoryCache _cache;

    public UserRepository(
        FanficsDbContext context,
        UserManager<User> userManager,
        ILogger<UserRepository> logger,
        IMemoryCache cache)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IdentityResult> RegisterUserAsync(User user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return result;
        }
        
        var roleAddingResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleAddingResult.Succeeded)
        {
            _logger.LogError("Error(s) occured while adding user {UserId} to role {Role}: {ErrorsList}", 
                user.Id, role, string.Join(", ", result.Errors.Select(err => $"{err.Code}: {err.Description}")));
        }

        return result;
    }

    public async Task<User?> GetAsync(string idOrUserName, bool asNoTracking = true)
    {
        var usersQuery = _userManager.Users
            .Include(u => u.FanficCoauthors)
            .Include(u => u.Fanfics)
            .Include(u => u.CoauthoredFanfics)
            .Include(u => u.FanficComments)
            .Include(u => u.FanficCommentReactions)
            .AsQueryable();

        if (asNoTracking)
        {
            usersQuery = usersQuery.AsNoTracking().AsQueryable();
        }

        Expression<Func<User, bool>> searchExpression;

        if (Guid.TryParse(idOrUserName, out var id))
        {
            searchExpression = u => u.Id == id.ToString();
        }
        else
        {
            searchExpression = u => u.UserName != null && u.UserName.Equals(idOrUserName);
        }

        return await usersQuery.FirstOrDefaultAsync(searchExpression);
    }

    public async Task<List<User>> GetRangeAsync(List<string> userIds) =>
        await _userManager.Users
            .Where(u => !u.IsBlocked && userIds.Contains(u.Id))
            .ToListAsync();

    public async Task<List<User>> GetListAsync(string? userName = null)
    {
        var isSearchByName = !string.IsNullOrWhiteSpace(userName);
        var usersQuery = _userManager.Users
            .AsNoTracking()
            .Where(u => !u.IsBlocked);
        
        if (isSearchByName)
        {
            usersQuery = usersQuery.Where(u =>
                u.UserName!.Contains(userName!));
        }
        return await usersQuery
            .OrderBy(u => u.UserName)
            .ToListAsync();
    }

    public async Task<bool> CheckPasswordAsync(User user, string password) =>
        await _userManager.CheckPasswordAsync(user, password);

    public async Task<bool> UserExistsAsync(string idOrUserName) =>
        await _userManager.Users.AnyAsync(u => u.Id == idOrUserName
                                               || (!string.IsNullOrEmpty(u.UserName)
                                                   && u.UserName.Contains(idOrUserName)));

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
        await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);

    public async Task<List<string>> GetRolesAsync(User user)
    {
        var users = await _userManager.GetRolesAsync(user);
        return users.ToList();
    }

    public async Task<IdentityResult> UpdateAsync(User user) =>
        await _userManager.UpdateAsync(user);

    public async Task<bool> DeleteAsync(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.FanficCommentReactions.RemoveRange(user.FanficCommentReactions);
            _context.FanficComments.RemoveRange(user.FanficComments);
            _context.FanficCoauthors.RemoveRange(user.FanficCoauthors);
            _context.Fanfics.RemoveRange(user.Fanfics);
            var deleteResult = await _userManager.DeleteAsync(user);
            await transaction.CommitAsync();
            return deleteResult.Succeeded;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A DB error occured when deleting a user.");
            await transaction.RollbackAsync();
            return false;
        }
    }

    public IQueryable<User> GetAll() => _userManager.Users
        .AsNoTracking()
        .Include(u => u.Fanfics)
        .Include(u => u.CoauthoredFanfics);
}