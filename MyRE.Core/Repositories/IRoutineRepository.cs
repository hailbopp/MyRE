using System;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public interface IRoutineRepository
    {
        Task<Block> GetStatementTree(Guid blockId);
    }
}