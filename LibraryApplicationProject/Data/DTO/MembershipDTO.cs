namespace LibraryApplicationProject.Data.DTO;

public class MembershipDTO : PersonDTO
{
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}

