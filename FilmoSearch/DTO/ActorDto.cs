namespace FilmoSearch.DTO
{
    public record ActorDto(Guid? Id, string FirstName, string LastName, List<FilmDto>? Films);
}
