using Battleship.Models;
using HotChocolate.Types;

namespace Battleship.GraphQL.Ships
{
    public class ShipType : ObjectType<Ship>
    {
        protected override void Configure(IObjectTypeDescriptor<Ship> descriptor)
        {
            descriptor.Description("Ships");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(s => s.Id);
            descriptor.Field(s => s.Name);
            descriptor.Field(s => s.Width);
            descriptor.Field(s => s.Hits);
            descriptor.Field(s => s.AlignmentType).Type<EnumType<AlignmentType>>();
            descriptor.Field(s => s.IsSunk);
        }
    }
}
