using Microsoft.EntityFrameworkCore;
using My.Custom.Template.Data.Entities.EF;
using My.Custom.Template.Data.Repositories.Interfaces;

namespace My.Custom.Template.Data.Repositories.Implementations;

public class ExampleRepository : BaseRepository<Example>, IExampleRepository
{
    public ExampleRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Example> GetExampleAsNoTrackingAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Set<Example>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}