using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;

namespace MyRE.Data.Repositories
{
    public class RoutineRepository: IRoutineRepository
    {
        private readonly MyREContext _dbContext;

        public RoutineRepository(MyREContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<Statement>  LoadStatement(Guid statementId, Statement statement)
        {
            Statement result = null;
            if (statement == null)
            {
                var stmt = await _dbContext.Statements.SingleAsync(s => s.StatementId == statementId);
                if (stmt is ActionStatement)
                {
                    var castStatement = stmt as ActionStatement;
                    return new ActionStatement()
                    {
                        StatementId = castStatement.StatementId,
                        Discriminator = castStatement.Discriminator,
                        ExpressionToEvaluate = castStatement.ExpressionToEvaluate ?? await _dbContext.Expressions.FindAsync(castStatement.ExpressionToEvaluateId)
                    };
                } else if (stmt is EventHandlerStatement)
                {
                    var castStatement = stmt as EventHandlerStatement;
                    return new EventHandlerStatement()
                    {
                        StatementId = castStatement.StatementId,
                        Discriminator = castStatement.Discriminator,
                        Block = castStatement.Block ?? await GetStatementTree(castStatement.Block.BlockId),
                        Event = castStatement.Event,
                    };
                }
            }

            return null;
        }

        public async Task<Block> GetStatementTree(Guid blockId)
        {
            return null;
        }
    }
}