using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class BoardSquare
    {
        [Key]
        public Guid Id { get; set; }
        public Coordinates Coordinates { get; set; }
        public bool IsOccupied { get; set; } = default!;
        public HitStatus HitStatus { get; set; } = default!;
        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        public Guid? ShipId { get; set; }
    }
}
