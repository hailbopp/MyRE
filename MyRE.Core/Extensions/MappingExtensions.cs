using Domain = MyRE.Core.Models.Domain;
using Data = MyRE.Core.Models.Data;

namespace MyRE.Core.Extensions
{
    public static class MappingExtensions
    {
        // Project
        public static Domain.Project ToDomainModel(this Data.Project self) => new Domain.Project()
        {
            ProjectId = self.ProjectId,
            Name = self.Name,
            Description = self.Description,
        };

        public static Data.Project ToDataModel(this Domain.Project self) => new Data.Project()
        {
            ProjectId = self.ProjectId,
            Name = self.Name,
            Description = self.Description,
        };

        // Instance
        public static Domain.Instance ToDomainModel(this Data.AppInstance self) => new Domain.Instance()
        {
            Id = self.AppInstanceId,
            Name = self.Name,
            AccountId = self.AccountId,
        };


    }
}
