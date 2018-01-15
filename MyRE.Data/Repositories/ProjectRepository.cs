using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Extensions;
using MyRE.Core.Models.Data;
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

            return results;
        }

        public async Task<Project> CreateAsync(string name, string description, Guid instanceId)
        {
            var newEntity = new Core.Models.Data.Project()
            {
                Name = name,
                Description = description,
                ParentInstanceId = instanceId
            };
            var createResult = await _dbContext.Projects.AddAsync(newEntity);
            var saveResult = await _dbContext.SaveChangesAsync();

            return createResult.Entity;
        }

        public async Task<Project> GetByIdAsync(Guid projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public async Task<ApplicationUser> GetOwnerAsync(Guid projectId)
        {
            return await _dbContext
                .Users.FirstOrDefaultAsync(u => u
                    .Accounts.Any(acc => acc
                        .AppInstances.Any(ai => ai
                            .Projects.Any(p => p.ProjectId == projectId))));
        }

        public async Task DeleteAsync(Guid projectId)
        {
            var entity = await GetByIdAsync(projectId);
            var deleteResult = _dbContext.Projects.Remove(entity);
            var saveResult = await _dbContext.SaveChangesAsync();
        }
    }
}
