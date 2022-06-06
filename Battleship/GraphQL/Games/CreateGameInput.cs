using Battleship.GraphQL.Players;
using Battleship.Models;
using System.ComponentModel.DataAnnotations;

namespace Battleship.GraphQL.Games
{
    public class CreateGameInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public PlayerInputType Player { get; set; }
    }
}
