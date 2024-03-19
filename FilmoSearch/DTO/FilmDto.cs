using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FilmoSearch.DTO
{
    public record FilmDto(Guid? Id, string Title, List<ReviewDto>? Reviews, List<ActorDto>? Actors);    
}
