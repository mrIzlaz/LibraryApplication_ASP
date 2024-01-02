using LibraryApplicationProject.Data.DTO;

namespace LibraryApplicationProject.Data;

public class Membership
{
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public DateOnly RegistryDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public Person? Person { get; set; }

    public bool IsStillValid(DateOnly date)
    {
        if (ExpirationDate == null) return false;
        if (Person == null) return false;
        return date <= ExpirationDate;

    }

}
