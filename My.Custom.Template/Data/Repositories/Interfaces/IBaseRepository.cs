
namespace My.Custom.Template.Data.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetAsync(object id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken);
    void Update(T entity);
    void UpdateRange(List<T> entities);
    void Delete(T entity);
    void DeleteRange(List<T> entities);
    Task SaveChangesAsync(CancellationToken cancellationToken);

}