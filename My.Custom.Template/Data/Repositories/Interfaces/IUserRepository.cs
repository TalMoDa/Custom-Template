using My.Custom.Template.Data.Entities;
using My.Custom.Template.Data.Entities.EF;

namespace My.Custom.Template.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
}