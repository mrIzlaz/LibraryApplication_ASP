namespace LibraryApplicationProject.Data
{
    public class Rating
    {
        public int Id { get; set; }
        public int ReaderRating { get; set; }
        public ISBN? Isbn { get; set; }
        public Membership? Membership { get; set; }
    }
}
