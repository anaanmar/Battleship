using Battleship.GameService;
using Battleship.GraphQL.Players;
using Battleship.Models;
using HotChocolate.Types;
using System;

namespace Battleship.GraphQL.Games
{
    public class GameType : ObjectType<Game>
    {
        protected override void Configure(IObjectTypeDescriptor<Game> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(g => g.Id);
            descriptor.Field(g => g.Name);
            descriptor.Field(g => g.NextTargetId);
            descriptor.Field(g => g.Players)
                .Description("The Players of the game")
                .Type<ListType<PlayerType>>()
                .Resolve(async ctx =>
                {
                    return await ctx.GroupDataLoader<Guid, Player>((keys, token) =>
                    ctx.Service<IService>().GetGamePlayersAsync(keys),
                    "gamePlayers").LoadAsync(ctx.Parent<Game>().Id, ctx.RequestAborted);
                }
                );
        }
    }
}
