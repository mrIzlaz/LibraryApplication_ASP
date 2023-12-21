namespace LibraryApplicationProject.Data.DTO;

public class AuthorDTORead : PersonDTO
{
    public string? Description { get; set; } = string.Empty;
    public List<string>? BooksList { get; set; } = new();
}

public class AuthorDTOInsert : PersonDTO
{
    public string? Description { get; set; } = string.Empty;
}

