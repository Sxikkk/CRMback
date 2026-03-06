using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAndPasswordAsync(string email, string hashPassword);
    Task<User?> GetUserByUsernameAndPasswordAsync(string username, string hashPassword);
    Task<IEnumerable<User>> GetAllUsersAsync();
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
    Task SaveAsync();
}