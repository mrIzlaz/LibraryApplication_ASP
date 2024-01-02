namespace LibraryApplicationProject.Data.DTO;
public class ISBNDTOEntry
{
    public int Id { get; set; }
    public long Isbn { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double AvgRating { get; set; } = 0;
    public DateOnly ReleaseDate { get; set; }
    public List<int> Authors { get; set; } = new();
}

public class ISBNDTORead
{
    public int Id { get; set; }
    public long Isbn { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double AvgRating { get; set; } = 0;
    public DateOnly ReleaseDate { get; set; }
    public List<string?> Authors { get; set; } = new();
}

