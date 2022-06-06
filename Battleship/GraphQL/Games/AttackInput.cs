using Battleship.Models;
using System;

namespace Battleship.GraphQL.Games
{
    public record AttackInput(Guid gameId, Battleship.Models.Coordinates Coordinates);
}
