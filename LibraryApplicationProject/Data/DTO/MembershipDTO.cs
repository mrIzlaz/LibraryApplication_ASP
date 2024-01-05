namespace LibraryApplicationProject.Data.DTO;

public class MembershipDTO : PersonDTOPost
{
    public int MembershipId { get; set; }
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public bool HasActiveLoan { get; set; }
    public void SetActiveLoan(bool hasActiveLoan) => HasActiveLoan = hasActiveLoan;
}

