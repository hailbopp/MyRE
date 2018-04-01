using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Extensions;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Language;
using MyRE.Core.Repositories;
using Newtonsoft.Json;

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

            var createNewSourceResult = await SetProjectSource(createResult.Entity.ProjectId, "", "[]");

            await _dbContext.Entry(newEntity).Reference(p => p.ParentInstance).LoadAsync();

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

        public async Task<Project> UpdateAsync(Project entity)
        {
            var existingProject = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

            existingProject.Name = entity.Name;
            existingProject.Description = entity.Description;

            var existingSource = await _dbContext.ProjectSourceVersions.OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync(s => s.ProjectId == entity.ProjectId);
            existingSource.ParsedExpressionTree = entity.Source.ParsedExpressionTree;
            existingSource.Source = entity.Source.Source;

            var saveResult = await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(existingProject).Reference(p => p.ParentInstance).LoadAsync();

            return existingProject;
        }

        public async Task<ProjectSource> SetProjectSource(Guid projectId, string source, string expressionTree)
        {
            var entity = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (entity == null)
            {
                return null;
            }

            var existingSource = await _dbContext.ProjectSourceVersions.OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync(s => s.ProjectId == projectId);

            if (existingSource == null)
            {
                var newSource = new ProjectSource()
                {
                    Project = entity,
                    Source = source,
                    ParsedExpressionTree = JsonConvert.DeserializeObject<List<Object>>(expressionTree)
                };

                var addResult = await _dbContext.ProjectSourceVersions.AddAsync(newSource);
                var result = await _dbContext.SaveChangesAsync();

                return addResult.Entity;
            }
            else
            {
                existingSource.Source = source;
                existingSource.ParsedExpressionTree = JsonConvert.DeserializeObject<List<Object>>(expressionTree);

                await _dbContext.SaveChangesAsync();

                return existingSource;
            }
        }

        public async Task<ProjectSource> GetSourceById(Guid projectId)
        {
            return await _dbContext.ProjectSourceVersions.OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync(s => s.ProjectId == projectId);
        }
    }
}
