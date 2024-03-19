namespace FilmoSearch.Models
{
    public class Film
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public ICollection<Actor>? Actors { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
