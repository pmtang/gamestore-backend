using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
        new(1, "civilization I", "strategy", 19.99M, new DateOnly(2001,1,1)),
        new(2, "civilization II", "strategy", 29.99M, new DateOnly(2002,2,2)),
        new(3, "civilization III", "strategy", 39.99M, new DateOnly(2003,3,3))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        //group the route
        var group = app.MapGroup("/games")
                    .WithParameterValidation();

        //Get /games
        group.MapGet("/", () => games);

        //Get /game/id
        group.MapGet("/{id}", (int id) => 
        { 
            GameDto? game = games.Find(game => game.Id == id);
            return game == null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        //Post /games
        group.MapPost("/", (CreateGameDto newGame) => {
            
            //instead using the following to check -> data annotation
            // if(string.IsNullOrEmpty(newGame.Name)){
            //     return Results.BadRequest("Name is required");
            // }

            GameDto game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new{ id = game.Id}, game);
        });

        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => 
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;
    }
}
