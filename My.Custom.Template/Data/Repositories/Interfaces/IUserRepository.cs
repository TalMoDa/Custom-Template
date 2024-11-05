﻿using My.Custom.Template.Data.Entities.EF;

namespace My.Custom.Template.Data.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserAsNoTrackingAsync(int id, CancellationToken cancellationToken);
}