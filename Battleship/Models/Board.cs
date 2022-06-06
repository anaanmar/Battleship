using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class Board
    {
        [Key]
        public Guid Id { get; set; }
        public List<BoardSquare> BoardPoints { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
        
    }
}
