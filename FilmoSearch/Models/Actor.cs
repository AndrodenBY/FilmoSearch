using System.Text.Json.Serialization;

namespace FilmoSearch.Models
{
    public class Actor
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public ICollection<Film>? Films {  get; set; } 
    }
}
