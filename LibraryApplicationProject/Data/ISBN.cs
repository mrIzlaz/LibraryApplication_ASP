using System.ComponentModel.DataAnnotations;

namespace LibraryApplicationProject.Data;

public class ISBN
{
    public int Isbn_Id { get; set; }
    public long Isbn { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public List<Author> Author { get; set; } = new List<Author>();
    public override string ToString() => $"{Title}, {Description}, {ReleaseDate}, Isbn: {Isbn},\n " +
                                         $"{Author.Aggregate(string.Empty, (s, a) => s + $"{a.Person?.FirstName} {a.Person?.LastName} \n")}";

}

