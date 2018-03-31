using Optional;
using Domain = MyRE.Core.Models.Domain;
using Data = MyRE.Core.Models.Data;

namespace MyRE.Core.Extensions
{
    public static class MappingExtensions
    {
        // Instance
        public static Domain.Instance ToDomainModel(this Data.AppInstance self) => new Domain.Instance()
        {
            InstanceId = self.AppInstanceId,
            Name = self.Name,
            AccountId = self.AccountId,
        };
        
    }
}

