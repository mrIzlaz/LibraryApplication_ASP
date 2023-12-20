namespace LibraryApplicationProject.Data;

public class Book
{
    public int Id { get; set; }
    public ISBN? Isbn { get; set; }
    public bool IsAvailable { get; set; }
}

