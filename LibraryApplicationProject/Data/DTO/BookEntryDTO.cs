namespace LibraryApplicationProject.Data.DTO
{
    public class BookDTO : ISBNDTO
    {
        public bool IsAvailable { get; set; }
    }
    public class BookEntryDTO : BookDTO
    {
        public int Quantity { get; set; }
    }

    public class BookSearchDTO : ISBNDTO
    {
        public int Quantity { get; set; }
        public int Available { get; set; }
    }
}
