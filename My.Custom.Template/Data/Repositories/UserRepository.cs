using Microsoft.EntityFrameworkCore;
using My.Custom.Template.Data.Entites;
using My.Custom.Template.Data.Repositories.Interfaces;

namespace My.Custom.Template.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CustomDbContext _context;
    
    public UserRepository(CustomDbContext context)
    {
        _context = context;
    }


    public async Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(id, cancellationToken);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}