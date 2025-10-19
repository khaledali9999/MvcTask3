namespace MvcTask3.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public bool Status { get; set; }

        public DateTime dateTime { get; set; }
        public List<string>? SubImages { get; set; } 

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        public required List<Actor> ActorMovies { get; set; }

    }
}
