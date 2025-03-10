using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    
    //in memory list is no longer needed
    // private static readonly List<GameSummaryDto> games = [
    //     new(1, "civilization I", "strategy", 19.99M, new DateOnly(2001,1,1)),
    //     new(2, "civilization II", "strategy", 29.99M, new DateOnly(2002,2,2)),
    //     new(3, "civilization III", "strategy", 39.99M, new DateOnly(2003,3,3))
    // ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        //group the route
        var group = app.MapGroup("/games")
                    .WithParameterValidation();

        //Get /games
        group.MapGet("/", (GameStoreContext dbContext) => 
        dbContext.Games
                 .Include(game => game.Genre)
                 .Select(game => game.ToGameSummaryDto())
                 .AsNoTracking() // for optimization, by tell ef there is nothing to track for changes, just to get infos 
                 );

        //Get /game/id
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) => 
        { 
            Game? game = dbContext.Games.Find(id);
            return game == null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

        //Post /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            
            //instead using the following to check -> data annotation
            // if(string.IsNullOrEmpty(newGame.Name)){
            //     return Results.BadRequest("Name is required");
            // }

            Game game = newGame.ToEntity();
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            return Results.CreatedAtRoute(GetGameEndpointName, new{ id = game.Id}, game.ToGameDetailsDto());
        });

        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) => 
        {
            dbContext.Games
                     .Where(game => game.Id == id) //batch delete; very efficient
                     .ExecuteDelete();
            return Results.NoContent();
        });

        return group;
    }
}
