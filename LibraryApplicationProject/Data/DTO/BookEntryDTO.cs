namespace LibraryApplicationProject.Data.DTO
{
    public class BookDTO : ISBNDTOEntry
    {
        public bool IsAvailable { get; set; }
    }
    public class BookEntryDTO : BookDTO
    {
        public int Quantity { get; set; }
    }

    public class BookSearchDTO : ISBNDTORead
    {

        //TODO: Replace inheritance with proper data? Just to get dataRead look good
        public int Quantity { get; set; }
        public int Available { get; set; }
    }
}
