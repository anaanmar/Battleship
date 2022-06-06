using Battleship.Models;

namespace Battleship.GraphQL.Players
{
    public class PlayerInputType
    {
        public string Name { get; set; }
        public Player ToPlayer()
        {
            return new Player
            {
                Name = Name,
                HasLost = false,
            };
        }
    }
}
