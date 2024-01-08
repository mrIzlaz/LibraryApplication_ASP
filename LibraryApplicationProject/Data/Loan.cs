using LibraryApplicationProject.Migrations;

namespace LibraryApplicationProject.Data;

public class Loan
{
    public int Id { get; set; }
    public Membership? Membership { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; }

    public void CloseLoan()
    {
        IsActive = false;
        foreach (var bookId in Books) bookId.IsAvailable = true;
        EndDate = DateOnly.FromDateTime(DateTime.Today);
    }

}

