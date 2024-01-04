namespace LibraryApplicationProject.Data.DTO
{
    public class AuthorBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long Isbn { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Quantity { get; set; }
        public List<AuthorDTOInsert> Authors { get; set; } = new();
    }

}
