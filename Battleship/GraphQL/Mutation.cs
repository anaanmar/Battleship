using Battleship.GameService;
using Battleship.GraphQL.Games;
using Battleship.GraphQL.Ships;
using HotChocolate;
using System.Threading.Tasks;

namespace Battleship.GraphQL
{
    public class Mutation
    {
        /// <summary>
        /// The main game setup
        /// 1- Create the game record if not exists 
        /// 2- Create the player and his board
        /// 3- add the player to the game
        /// </summary>
        /// <param name="input">includes the game name and the player name</param>
        /// <returns>the game object includes the player list</returns>
        public async Task<CreateGamePayload> createGame(CreateGameInput input, [Service] IService service)
        {
            return new CreateGamePayload(await service.CreateGameAsync(input));
        }

        /// <summary>
        /// Add Ship to the player board
        /// </summary>
        /// <param name="input">includes the Ship details (name , width , AlignmentType , first square coordinates , the player id)</param>
        /// <returns>the player object</returns>
        public async Task<AddShipPayload> addShipToPlayerBoard(AddShipInput input, [Service] IService service)
        {
            return new AddShipPayload(await service.AddShipAsync(input));
        }

        /// <summary>
        /// Take an attack
        /// </summary>
        /// <param name="input">includes the game id and the target coordinates</param>
        /// <returns>the hit status</returns>
        public async Task<HitStatus> attack(AttackInput input, [Service] IService service)
        {
            return await service.AttackAsync(input);
        }
    }
}
