namespace GameStore.Api.Dtos;

public record class GameDto(int Id, string name, string Genre, decimal Price, DateOnly ReleaseDate);