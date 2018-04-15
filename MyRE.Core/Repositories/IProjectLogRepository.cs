using System;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public interface IProjectLogRepository : IEntityRepository<ProjectLog, Guid>
    {
    }
}