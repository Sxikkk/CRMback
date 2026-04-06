using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string hashPassword, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hashPassword, cancellationToken);
    public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string hashPassword, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == hashPassword, cancellationToken);
    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken) => await _context.Users.ToListAsync(cancellationToken: cancellationToken);
    public async Task<IEnumerable<User>> GetAllUsersByOrgIdAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        return await _context.Users.Where(u => u.OrganizationId == organizationId).Distinct().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken) => await _context.Users.AddAsync(user, cancellationToken);
    public async Task<bool> IsExistByIdAsync(Guid userId, CancellationToken cancellationToken) => await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    public async Task<bool> IsExistByUsernameAsync(string username, CancellationToken cancellationToken) => await _context.Users.AnyAsync(u => u.UserName == username, cancellationToken);
    public void UpdateUser(User user) => _context.Users.Update(user);
    public void DeleteUser(User user) => _context.Users.Remove(user);
}
