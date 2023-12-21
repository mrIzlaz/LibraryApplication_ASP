namespace LibraryApplicationProject.Data.DTO;
public class ISBNDTO
{
    public int Id { get; set; }
    public long Isbn { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public List<string?> Authors { get; set; } = new();
}
