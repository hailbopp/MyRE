using System;
using System.Collections.Generic;
using System.Text;
using Domain = MyRE.Core.Models.Domain;
using Data = MyRE.Core.Models.Data;

namespace MyRE.Core.Extensions
{
    public static class MappingExtensions
    {
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
    }
}
