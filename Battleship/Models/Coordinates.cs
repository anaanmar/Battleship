using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    [Owned]
    public class Coordinates
    {
        [Range(1,10)]
        public int Row { get; set; }

        [Range(1,10)]
        public int Column { get; set; }
        
    }
}
