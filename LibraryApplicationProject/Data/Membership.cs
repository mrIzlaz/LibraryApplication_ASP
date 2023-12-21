using LibraryApplicationProject.Data.DTO;

namespace LibraryApplicationProject.Data;

public class Membership
{
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public DateTime RegistryDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Person? Person { get; set; }
}
