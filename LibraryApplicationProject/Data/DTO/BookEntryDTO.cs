namespace LibraryApplicationProject.Data.DTO
{
    public class BookDTO : ISBNDTOEntry
    {
        public bool IsAvailable { get; set; }
    }
    public class BookEntryDTO : ISBNDTOEntry
    {
        public int Quantity { get; set; }
    }
    public class BookDTORead : ISBNDTORead
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class BookSearchDTO : ISBNDTORead
    {
        public int Quantity { get; set; }
        public int Available { get; set; }
    }
}
