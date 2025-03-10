namespace GameStore.Api.Dtos;

public record class GameDetailsDto(int Id, string name, int GenreId, decimal Price, DateOnly ReleaseDate);