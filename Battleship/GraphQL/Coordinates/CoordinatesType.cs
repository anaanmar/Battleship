using HotChocolate.Types;

namespace Battleship.GraphQL.Coordinates
{
    public class CoordinatesType: ObjectType<Battleship.Models.Coordinates>
    {
        protected override void Configure(IObjectTypeDescriptor<Models.Coordinates> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Column);
            descriptor.Field(c => c.Row);
        }
    }
}
