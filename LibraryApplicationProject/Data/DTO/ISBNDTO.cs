namespace LibraryApplicationProject.Data.DTO;
public class ISBNDTO
{
    public long Isbn_Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public List<string> Author { get; set; } = new List<string>();
}
