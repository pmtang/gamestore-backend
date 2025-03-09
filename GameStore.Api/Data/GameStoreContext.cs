using System;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options): DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Ge1"},
            new { Id = 2, Name = "Ge2"},
            new { Id = 3, Name = "Ge3"},
            new { Id = 4, Name = "Ge4"},
            new { Id = 5, Name = "Ge5"}
        );
    }
}
