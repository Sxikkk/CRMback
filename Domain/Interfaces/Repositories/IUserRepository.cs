using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAndPasswordAsync(string email, string hashPassword, CancellationToken cancellationToken);
    Task<User?> GetUserByUsernameAndPasswordAsync(string username, string hashPassword, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    void UpdateUser(User user);
    void DeleteUser(User user);
    Task SaveAsync(CancellationToken cancellationToken);
}