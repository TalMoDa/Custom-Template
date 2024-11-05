using My.Custom.Template.Data.Entities.EF;

namespace My.Custom.Template.Data.Repositories.Interfaces;

public interface IExampleRepository : IBaseRepository<Example>
{
    Task<Example> GetExampleAsNoTrackingAsync(int id, CancellationToken cancellationToken);
}