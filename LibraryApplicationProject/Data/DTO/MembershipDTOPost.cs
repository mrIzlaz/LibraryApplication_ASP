namespace LibraryApplicationProject.Data.DTO;
public class MembershipDTOPost : PersonDTOPost
{
    public int MembershipId { get; set; }
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public bool HasActiveLoan { get; set; }
}

public class MembershipDTORead : PersonDTORead
{
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}

