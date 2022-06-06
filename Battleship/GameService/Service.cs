using Battleship.Caching;
using Battleship.GraphQL.Games;
using Battleship.GraphQL.Ships;
using Battleship.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.GameService
{
    public class Service : IService
    {
        private readonly BattleShipDbContext DbContext;

        public Service(BattleShipDbContext battleShipDbContext)
        {
            DbContext = battleShipDbContext;
        }
        public async Task<Game> CreateGameAsync(CreateGameInput input)
        {
            var game = await DbContext.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Name == input.Name);
            if (game != null)
            {
                if (game.Players != null && game.Players.Count == 2)
                    throw new Exception("this game already has 2 players");
                if (game.Players != null && game.Players.Count <= 1)
                {
                    if (game.Players.Any(p => p.Name == input.Player.Name))
                        throw new Exception("this player has already joined this game");

                    var player = input.Player.ToPlayer();
                    DbContext.Players.Add(player);
                    CreateBoard(player);
                    game.Players.Add(player);
                    game.NextTargetId = player.Id;
                }

                DbContext.Games.Update(game);
                await DbContext.SaveChangesAsync();
            }
            else
            {
                game = new Game
                {
                    Name = input.Name,
                    Players = new List<Player>()
                };
                if (input.Player == null)
                    throw new Exception("please pass the Player details");
                var player = input.Player.ToPlayer();
                DbContext.Players.Add(player);
                CreateBoard(player);
                game.Players.Add(player);
                game.NextTargetId = player.Id;
                DbContext.Games.Add(game);
                await DbContext.SaveChangesAsync();
            }
            return game;

        }

        private void CreateBoard(Player player)
        {
            var BoardPoints = new List<BoardSquare>();
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var newPoint = new BoardSquare
                    {
                        Coordinates = new Battleship.Models.Coordinates { Row = i, Column = j },
                        HitStatus = HitStatus.NOT_HITTED,
                        IsOccupied = false
                    };

                    BoardPoints.Add(newPoint);
                }
            }
            DbContext.BoardSquares.AddRange(BoardPoints);
            var newBoard = new Board { BoardPoints = BoardPoints };
            DbContext.Boards.Add(newBoard);
            player.Board = newBoard;
        }
        public IQueryable<Game> Games() => DbContext.Games;

        public async Task<ILookup<Guid, Player>> GetGamePlayersAsync(IReadOnlyList<Guid> gameIds)
        {
            return (await DbContext.Players.Where(p => gameIds.Contains(p.GameId)).ToArrayAsync()).ToLookup(p => p.GameId);
        }
        public async Task<ILookup<Guid, BoardSquare>> GetBoardSquaresAsync(IReadOnlyList<Guid> boardIds)
        {
            return (await DbContext.BoardSquares.Where(p => boardIds.Contains(p.BoardId)).ToArrayAsync()).ToLookup(p => p.BoardId);
        }

        public async Task<ILookup<Guid, Ship>> GetPlayerShipsAsync(IReadOnlyList<Guid> playerIds)
        {
            return (await DbContext.Ships.Where(p => playerIds.Contains(p.PlayerId)).ToArrayAsync()).ToLookup(p => p.PlayerId);
        }

        public async Task<Board> GetPlayerBoardAsync(Guid playerId)
        {
            return await DbContext.Boards.FirstOrDefaultAsync(b => b.PlayerId == playerId);
        }
        public async Task<Player> AddShipAsync(AddShipInput input)
        {
            var player = await DbContext.Players
                .Include(p => p.Board)
                    .ThenInclude(b => b.BoardPoints)
                .Include(p => p.Ships)
                .FirstOrDefaultAsync(p => p.Id == input.PlayerId);

            if (player == null)
                throw new Exception("this player could not be found");

            if (input.Width > 10)
                throw new Exception("the width should be less than or equal to 10");

            if (input.Coordinates.Column > 10 || input.Coordinates.Row > 10)
                throw new Exception("incorrect coordinations");

            //var newShip = new Ship(input.Name, input.width, input.alignmentType);
            var newShip = new Ship()
            {
                Name = input.Name,
                AlignmentType = input.AlignmentType,
                Hits = 0,
                PlayerId = input.PlayerId,
                Width = input.Width,
                IsSunk = false
            };
            DbContext.Ships.Add(newShip);
            switch (input.AlignmentType)
            {
                case AlignmentType.VERTICAL:
                    if ((input.Coordinates.Row + input.Width - 1) > 10)
                        throw new Exception("We cannot place ships beyond the boundaries of the board");

                    var verticalShipBoardPoints = player.Board.BoardPoints.Where(x => x.Coordinates.Row >= input.Coordinates.Row
                                     && x.Coordinates.Row < (input.Coordinates.Row + input.Width)
                                     && x.Coordinates.Column == input.Coordinates.Column).ToList();

                    if (verticalShipBoardPoints.Any(p => p.IsOccupied))
                        throw new Exception("failed to add this ship , some squares already occupied");

                    foreach (var boardSquare in verticalShipBoardPoints)
                    {
                        boardSquare.IsOccupied = true;
                        boardSquare.ShipId = newShip.Id;
                    }
                    break;

                case AlignmentType.HORIZONTAL:
                    if ((input.Coordinates.Column + input.Width - 1) > 10)
                        throw new Exception("We cannot place ships beyond the boundaries of the board");

                    var horizontalShipBoardPoints = player.Board.BoardPoints.Where(x => x.Coordinates.Column >= input.Coordinates.Column
                                     && x.Coordinates.Column < (input.Coordinates.Column + input.Width)
                                     && x.Coordinates.Row == input.Coordinates.Row).ToList();

                    if (horizontalShipBoardPoints.Any(p => p.IsOccupied))
                        throw new Exception("failed to add this ship , some squares already occupied");

                    foreach (var boardSquare in horizontalShipBoardPoints)
                    {
                        boardSquare.IsOccupied = true;
                        boardSquare.ShipId = newShip.Id;
                    }
                    break;
            }

            await DbContext.SaveChangesAsync();
            return player;
        }

        public async Task<HitStatus> AttackAsync(AttackInput input)
        {
            var game = await DbContext.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.Board)
                        .ThenInclude(b => b.BoardPoints)
                .Include(g => g.Players)
                    .ThenInclude(p => p.Ships)
                .FirstOrDefaultAsync(g => g.Id == input.gameId);

            if (game == null)
                throw new Exception("this game could not be found");
            if (game.Players.Any(p => p.Ships == null))
                throw new Exception("one of the players hasn't added ship(s) to his board yet");
            var attcker = game.Players.FirstOrDefault(p => p.Id != game.NextTargetId);
            var target = game.Players.FirstOrDefault(p => p.Id != game.NextTargetId);


            var point = target.Board.BoardPoints.FirstOrDefault(p => p.Coordinates.Row == input.Coordinates.Row && p.Coordinates.Column == input.Coordinates.Column);
            if (point.HitStatus != HitStatus.NOT_HITTED)
                throw new Exception("you have attacked these coordinates before");
            if (point.IsOccupied)
            {
                point.HitStatus = HitStatus.HIT;
                var hittedShip = target.Ships.FirstOrDefault(s => s.Id == point.ShipId);
                hittedShip.Hits++;
                hittedShip.IsSunk = hittedShip.Hits >= hittedShip.Width;
                target.HasLost = target.Ships.All(s => s.IsSunk);
            }
            else
                point.HitStatus = HitStatus.MISS;
            game.NextTargetId = attcker.Id;
            await DbContext.SaveChangesAsync();
            return point.HitStatus;
        }

    }

    public interface IService
    {
        public Task<Game> CreateGameAsync(CreateGameInput input);
        public Task<Player> AddShipAsync(AddShipInput input);
        public Task<HitStatus> AttackAsync(AttackInput input);

        public IQueryable<Game> Games();
        public Task<ILookup<Guid, Player>> GetGamePlayersAsync(IReadOnlyList<Guid> gameIds);
        public Task<Board> GetPlayerBoardAsync(Guid playerId);

        public Task<ILookup<Guid, BoardSquare>> GetBoardSquaresAsync(IReadOnlyList<Guid> boardIds);
        public Task<ILookup<Guid, Ship>> GetPlayerShipsAsync(IReadOnlyList<Guid> playerIds);
    }
}
