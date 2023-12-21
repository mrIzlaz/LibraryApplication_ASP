namespace LibraryApplicationProject.Data.DTO;

public class MembershipDTO : PersonDTO
{
    public long CardNumber { get; set; }
    public DateTime RegistryDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

