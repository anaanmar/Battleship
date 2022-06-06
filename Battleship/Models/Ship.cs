using HotChocolate;
using System;
using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class Ship
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Ship's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of squares the ship needs on the game board
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Number of hits
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// Alignment type (vertical or horizontal)
        /// </summary>
        public AlignmentType AlignmentType { get; set; }

        /// <summary>
        /// Ship is sunk if it has been hit on all the squares it occupies
        /// </summary>
        public bool IsSunk { get; set; } = default!;
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

    }
}
