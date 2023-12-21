namespace LibraryApplicationProject.Data.DTO
{
    public class LoanDTOEntry
    {
        public int Id { get; set; }
        public long MembershipCardNumber { get; set; }
        public List<int> BookIds { get; set; } = new();
        public DateOnly StartDate { get; set; }
        public DateOnly ReturnDate { get; set; }
    }

    public class LoanDTORead
    {
        public int Id { get; set; }
        public long MembershipCardNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<BookDTO> Books { get; set; } = new();
        public DateOnly StartDate { get; set; }
        public DateOnly ReturnDate { get; set; }
    }
}
