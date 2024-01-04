namespace LibraryApplicationProject.Data.DTO
{
    public class RatingDTOEntry
    {
        public int Rating { get; set; }
        public long MembershipCard { get; set; }
        public long Isbn { get; set; }
    }

    public class AggregateRatingDTORead
    {
        public int NoRatings { get; set; }
        public ISBNDTORead IsbnDto { get; set; } = null!;
    }
    public class SingleRatingDTORead
    {
        public int Rating { get; set; }
        public int MembershipId { get; set; }
        public long Isbn { get; set; }
    }
}
