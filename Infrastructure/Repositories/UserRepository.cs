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
    
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email);

    public async Task<User?> GetUserByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string hashPassword) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email && u.PasswordHash == hashPassword);

    public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string hashPassword) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == hashPassword);

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();

    public void AddUser(User user) => _context.Users.Add(user);

    public void UpdateUser(User user) => _context.Users.Update(user);

    public void DeleteUser(User user) => _context.Users.Remove(user);

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
