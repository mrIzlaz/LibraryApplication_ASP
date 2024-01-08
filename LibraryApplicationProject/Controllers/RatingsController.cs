using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public RatingsController(LibraryDbContext context)
        {
            _context = context;
        }

        //GET: api/Ratings/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<SingleRatingDTORead>>> GetAllRatings()
        {
            var rs = await _context.Rating
                 .Include(p => p.Isbn)
                 .Include(p => p.Isbn.Author)
                 .ThenInclude(a => a.Person)
                 .Include(p => p.Membership)
                 .AsNoTracking()
                 .ToListAsync();
            return rs.ConvertToSingleDtoList();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SingleRatingDTORead>> GetRating(int id)
        {
            var rs = await _context.Rating
                .Include(p => p.Isbn)
                .Include(p => p.Isbn.Author)
                .ThenInclude(a => a.Person)
                .Include(p => p.Membership)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            if (rs == null)
                return NotFound();
            return rs.ConvertToSingleDto();
        }



        // GET: api/Ratings/member/5
        [HttpGet("member/{id:int}")]
        public async Task<ActionResult<IEnumerable<SingleRatingDTORead>>> GetRatingByMember(int id)
        {
            List<SingleRatingDTORead> sList = new List<SingleRatingDTORead>();
            var rating = await _context.Rating
                .Include(r => r.Isbn)
                .Include(r => r.Membership)
                .AsNoTracking()
                .Where(i => i.Membership != null && i.Membership.Id == id).ToListAsync();

            rating.ForEach(r => sList.Add(r.ConvertToSingleDto()));

            return sList;
        }


        // GET: api/Ratings/isbn/52423432
        [HttpGet("isbn/{isbn:long}")]
        public async Task<ActionResult<AggregateRatingDTORead>> GetRatingForBook(long isbn)
        {
            var rating = await _context.Rating
                .Include(p => p.Isbn)
                .Include(i => i.Isbn.Author)
                .ThenInclude(a => a.Person)
                .AsNoTracking()
                .Where(i => i.Isbn.Isbn == isbn).ToListAsync();
            var hasRating = rating.Count != 0;
            var avgRating = hasRating ? Math.Round(rating.Average(p => p.ReaderRating), 2) : 0;
            var dto = new AggregateRatingDTORead()
            {
                NoRatings = hasRating ? rating.Count() : 0,
                IsbnDto = _context.ISBNs.Single(i => i.Isbn == isbn).ConvertToDto(avgRating),
            };

            return dto;
        }
        // POST: api/Ratings/new/0-5/isbn/1234567890/member/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new/{ratingVal:int}/isbn/{isbn:long}/member/{id:int}")]
        public async Task<ActionResult<SingleRatingDTORead>> PostOrPutRating(int id, long isbn, int ratingVal)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return NotFound("Membership not found");

            var isbnObj = await _context.ISBNs.SingleAsync(i => i.Isbn == isbn);

            if (ratingVal is < 0 or > 5) return BadRequest($"Rating value not within range (0-5), was {ratingVal}");

            var rating = await _context.Rating
                .FirstOrDefaultAsync(r => r.Isbn != null && r.Membership != null && r.Membership.Id == id && r.Isbn.Isbn == isbnObj.Isbn);

            if (rating != null)
            {
                // Update existing rating
                rating.ReaderRating = ratingVal;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return NoContent();
            }
            else
            {
                // Create new rating
                rating = new Rating()
                {
                    Membership = membership,
                    Isbn = isbnObj,
                    ReaderRating = ratingVal
                };

                _context.Rating.Add(rating);

                await _context.SaveChangesAsync();

                var dto = rating.ConvertToSingleDto();

                return CreatedAtAction("GetRating", new { id = rating.Id }, dto);
            }
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
