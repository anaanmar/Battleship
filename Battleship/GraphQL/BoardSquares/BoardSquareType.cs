using Battleship.Models;
using HotChocolate.Types;

namespace Battleship.GraphQL.BoardSquares
{
    public class BoardSquareType :ObjectType<BoardSquare>
    {
        protected override void Configure(IObjectTypeDescriptor<BoardSquare> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(bp => bp.Coordinates).Type<Battleship.GraphQL.Coordinates.CoordinatesType>();
            descriptor.Field(bp => bp.HitStatus).Type<EnumType<HitStatus>>();
            descriptor.Field(bp => bp.IsOccupied);
            descriptor.Field(bp => bp.ShipId);
        }
    }
}
