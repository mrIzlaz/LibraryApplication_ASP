namespace LibraryApplicationProject.Data.DTO
{
    public class RatingDTOEntry
    {
        public int Rating { get; set; }
        public long MembershipCard { get; set; }
        public long Isbn { get; set; }
    }

    public class RatingDTORead
    {
        public double AvgRating { get; set; }
        public int NoRatings { get; set; }
        public ISBNDTOEntry IsbnDto { get; set; } = null!;
    }
}
