using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Extensions;
using MyRE.Core.Models.Domain;
using MyRE.Core.Repositories;

namespace MyRE.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly MyREContext _dbContext;

        public ProjectRepository(MyREContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(string userId)
        {
            var results = await _dbContext.Projects.Where(p => p.ParentInstance.Account.UserId == userId).ToListAsync();

            return results.Select(r => r.ToDomainModel()).ToList();
        }

        public async Task<Project> CreateAsync(string name, string description, long instanceId)
        {
            var newEntity = new Core.Models.Data.Project()
            {
                Name = name,
                Description = description,
                ParentInstanceId = instanceId
            };
            var createResult = await _dbContext.Projects.AddAsync(newEntity);
            var saveResult = await _dbContext.SaveChangesAsync();

            return createResult.Entity.ToDomainModel();
        }
    }
}
