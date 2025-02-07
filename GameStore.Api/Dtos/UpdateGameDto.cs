namespace GameStore.Api.Dtos;

public record class UpdateGameDto(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);
