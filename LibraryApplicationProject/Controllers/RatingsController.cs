using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApplicationProject;
using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;
using System.Diagnostics.Eventing.Reader;

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

        //GET: api/Ratings/getall
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<SingleRatingDTORead>>> GetAllRatings()
        {
            var rs = await _context.Rating
                 .Include(p => p.Isbn)
                 .Include(p => p.Isbn.Author)
                 .ThenInclude(a => a.Person)
                 .Include(p => p.Membership)
                 .ToListAsync();
            return rs.ConvertToSingleDtoList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SingleRatingDTORead>> GetRating(int id)
        {
            var rs = await _context.Rating
                .Include(p => p.Isbn)
                .Include(p => p.Isbn.Author)
                .ThenInclude(a => a.Person)
                .Include(p => p.Membership)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (rs == null)
                return NotFound();
            return rs.ConvertToSingleDto();
        }



        // GET: api/Ratings/searchwith/5
        [HttpGet("searchwithmemberid{memberId}")]
        public async Task<ActionResult<IEnumerable<SingleRatingDTORead>>> GetRatingByMember(int memberId)
        {
            List<SingleRatingDTORead> sList = new List<SingleRatingDTORead>();
            var rating = await _context.Rating
                .Include(r => r.Isbn)
                .Include(r => r.Membership)
                .Where(i => i.Membership != null && i.Membership.Id == memberId).ToListAsync();

            if (rating == null)
            {
                return NotFound();
            }

            rating.ForEach(r => sList.Add(r.ConvertToSingleDto()));


            return sList;
        }


        // GET: api/Ratings/getratingforbook/52423432
        [HttpGet("getratingforbook{isbn}")]
        public async Task<ActionResult<AggregateRatingDTORead>> GetRatingForBook(int isbn)
        {
            var rating = await _context.Rating
                .Include(p => p.Isbn)
                .Include(i => i.Isbn.Author)
                .ThenInclude(a => a.Person)
                .Where(i => i.Isbn.Isbn == isbn).ToListAsync();
            bool hasRating = rating.Count != 0;
            var avgRating = hasRating ? Math.Round(rating.Average(p => p.ReaderRating), 2) : 0;
            var dto = new AggregateRatingDTORead()
            {
                NoRatings = hasRating ? rating.Count() : 0,
                IsbnDto = _context.ISBNs.Single(i => i.Isbn == isbn).ConvertToDto(avgRating),
            };

            if (rating == null)
            {
                return NotFound();
            }

            return dto;
        }
        // POST: api/Ratings/post/51/123412421/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("postorput/{memberId}/{isbn}/{ratingVal}")]
        public async Task<ActionResult<SingleRatingDTORead>> PostOrPutRating(int memberId, long isbn, int ratingVal)
        {
            var membership = await _context.Memberships.FindAsync(memberId);
            if (membership == null) return NotFound("Membership not found");

            var isbnObj = await _context.ISBNs.SingleAsync(i => i.Isbn == isbn);
            if (isbnObj == null) return NotFound("ISBN not found");

            if (ratingVal < 0 || ratingVal > 5) return BadRequest($"Rating value not within range (0-5), was {ratingVal}");

            var rating = await _context.Rating
                .FirstOrDefaultAsync(r => r.Membership.Id == memberId && r.Isbn.Isbn == isbnObj.Isbn);

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
        [HttpDelete("{id}")]
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
