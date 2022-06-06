using System.Linq;
using System.Threading.Tasks;
using Battleship.GameService;
using Battleship.Models;
using HotChocolate;
using Microsoft.Extensions.Caching.Memory;

namespace Battleship.GraphQL
{
    public class Query
    {
        /// <summary>
        /// get the game by name
        /// </summary>
        /// <param name="name">game name</param>
        /// <returns>game object</returns>
        public IQueryable<Game> getGame(string name, [Service] IService service)
        {
            return service.Games();
        }
    }
}
