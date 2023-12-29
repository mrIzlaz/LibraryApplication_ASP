namespace LibraryApplicationProject.Data;

public class Loan
{
    public int Id { get; set; }
    public Membership? Membership { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

}

