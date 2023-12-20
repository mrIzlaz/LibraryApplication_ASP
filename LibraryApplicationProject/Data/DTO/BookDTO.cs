namespace LibraryApplicationProject.Data.DTO
{
    public class BookDTO : ISBNDTO
    {
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
