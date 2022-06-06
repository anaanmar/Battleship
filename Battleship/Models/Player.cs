using HotChocolate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Battleship.Models
{
    /// <summary>
    /// Player Details
    /// </summary>
    public class Player
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /// <summary>
        /// Player name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// player lose if all his ships have been sunk.
        /// </summary>
        public bool HasLost { get; set; } 
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
        public List<Ship> Ships { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }

    }
}
