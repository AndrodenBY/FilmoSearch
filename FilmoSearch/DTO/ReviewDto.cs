using System.ComponentModel.DataAnnotations;

namespace FilmoSearch.DTO
{
    public record ReviewDto(Guid? Id, string Title, string Description, [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")] int Stars, FilmDto? Film);
}
