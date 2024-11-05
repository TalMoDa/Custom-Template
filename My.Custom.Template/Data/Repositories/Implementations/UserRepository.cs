using Microsoft.EntityFrameworkCore;
using My.Custom.Template.Data.Entities.EF;
using My.Custom.Template.Data.Repositories.Interfaces;

namespace My.Custom.Template.Data.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(CustomDbContext context) : base(context)
    {
    }

    public Task<User> GetUserAsNoTrackingAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}