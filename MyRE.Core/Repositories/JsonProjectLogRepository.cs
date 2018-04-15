using System;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public class JsonProjectLogRepository : JsonFileEntityRepository<ProjectLog, Guid>, IProjectLogRepository
    {
        public JsonProjectLogRepository(string directory): base(
            directory,
            pl => pl.ProjectLogId,
            (pl, i) => { pl.ProjectLogId = i; },
            keys => Guid.NewGuid(),
            Guid.Parse)
        {}
    }
}