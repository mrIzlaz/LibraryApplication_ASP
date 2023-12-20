namespace LibraryApplicationProject.Data.DTO;

public class MembershipDTO : PersonDTO
{
    public DateTime RegistryDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

