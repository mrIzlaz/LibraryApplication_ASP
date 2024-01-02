using Microsoft.Identity.Client;

namespace LibraryApplicationProject.Data.DTO;

public class AuthorDTORead : PersonDTO
{
    public new int Id { get; set; }
    public new string FirstName{ get; set; } = string.Empty;
    public new string LastName { get; set; } = string.Empty;
    public new DateOnly BirthDate { get; set; }
    public string? Description { get; set; } = string.Empty;
    public List<string>? BooksList { get; set; } = new();
}

public class AuthorDTOInsert : PersonDTO
{
    public string? Description { get; set; } = string.Empty;
}

