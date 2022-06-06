using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Battleship.Models
{
    /// <summary>
    /// Game details
    /// </summary>
    public class Game
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Game Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the target player for the next attack
        /// </summary>
        public Guid NextTargetId { get; set; } 

        /// <summary>
        /// Game players
        /// </summary>
        public List<Player> Players { get; set; }= default!;
    }
}
