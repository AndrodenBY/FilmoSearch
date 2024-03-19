using System.ComponentModel.DataAnnotations;

namespace FilmoSearch.Models
{
    public class Review
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Range(1,5)]
        public int Stars { get; set; }
        public Film? Film { get; set; } = null;
        public Guid? FilmId { get; set; }        
    }
}
