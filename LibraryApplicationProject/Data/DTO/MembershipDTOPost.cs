namespace LibraryApplicationProject.Data.DTO;
public class MembershipDTOPost : PersonDTOPost
{
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}

public class MembershipDTORead : PersonDTORead
{
    public int MembershipId { get; set; }
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public bool HasActiveLoan { get; set; }
}

