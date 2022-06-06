using Battleship.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server.Ui.Voyager;
using Battleship.GraphQL.Games;
using Battleship.GraphQL.Players;
using Battleship.GraphQL.Boards;
using Battleship.GraphQL.BoardSquares;
using Battleship.GraphQL.Ships;
using Battleship.GraphQL.Coordinates;
using Battleship.GameService;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Battleship.Caching;
using Microsoft.EntityFrameworkCore;

namespace Battleship
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IService, Service>();
            services.AddDbContext<BattleShipDbContext>(context =>
            {
                context.UseInMemoryDatabase("BattleShipServer");
            });
            services
                .AddGraphQLServer()
                .AddType<GameType>()
                .AddType<PlayerType>()
                .AddType<BoardType>()
                .AddType<BoardSquareType>()
                .AddType<ShipType>()
                .AddType<CoordinatesType>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
            app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
                
            },"/graphql-voyager");

            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql",
                Path = "/playground"
            });

        }
    }
}
