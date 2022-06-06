using Battleship.Models;
using System;

namespace Battleship.GraphQL.Ships
{
    public record AddShipInput(string Name,int Width, AlignmentType AlignmentType , Battleship.Models.Coordinates Coordinates, Guid PlayerId);
}
