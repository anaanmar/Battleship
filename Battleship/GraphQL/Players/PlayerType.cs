using Battleship.GameService;
using Battleship.GraphQL.Boards;
using Battleship.GraphQL.Ships;
using Battleship.Models;
using HotChocolate.Types;
using System;
using System.Linq;

namespace Battleship.GraphQL.Players
{
    public class PlayerType : ObjectType<Player>
    {
        protected override void Configure(IObjectTypeDescriptor<Player> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(s => s.Id);
            descriptor.Field(p => p.Name);
            descriptor.Field(p => p.HasLost);
            descriptor.Field(p => p.Board)
                .Type<BoardType>()
                .Resolve(async ctx =>
                {
                    return await ctx.Service<IService>().GetPlayerBoardAsync(ctx.Parent<Player>().Id);

                }
                );
            descriptor.Field(p => p.Ships)
                .Type<ListType<ShipType>>()
                .Resolve(async ctx =>
                {
                    return await ctx.GroupDataLoader<Guid, Ship>((keys, token) =>
                    ctx.Service<IService>().GetPlayerShipsAsync(keys),
                    "playerShips").LoadAsync(ctx.Parent<Player>().Id, ctx.RequestAborted);
                }
                );
        }
    }
}
