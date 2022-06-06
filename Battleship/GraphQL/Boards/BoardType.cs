using Battleship.GameService;
using Battleship.GraphQL.BoardSquares;
using Battleship.Models;
using HotChocolate.Types;
using System;

namespace Battleship.GraphQL.Boards
{
    public class BoardType : ObjectType<Board>
    {
        protected override void Configure(IObjectTypeDescriptor<Board> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(b => b.BoardPoints)
                .Type<ListType<BoardSquareType>>()
                .Resolve(async ctx =>
                {
                    return await ctx.GroupDataLoader<Guid, BoardSquare>((keys, token) =>
                    ctx.Service<IService>().GetBoardSquaresAsync(keys),
                    "boardSquares").LoadAsync(ctx.Parent<Board>().Id, ctx.RequestAborted);
                }
                ); ;
        }
    }
}
