namespace GameStore.Api.Dtos;

public record class GameSummaryDto(int Id, string name, string Genre, decimal Price, DateOnly ReleaseDate);