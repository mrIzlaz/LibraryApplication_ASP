namespace LibraryApplicationProject.Data.DTO
{
    public class BookEntryDTO : ISBNDTO
    {
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class BookDTO : ISBNDTO
    {
        public bool IsAvailable { get; set; }
    }
}
