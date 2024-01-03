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

        // GET: api/Ratings/searchwith/5
        [HttpGet("searchwithmemberid{memberId}")]
        public async Task<ActionResult<IEnumerable<SingleRatingDTORead>>> GetRatingByMember(int memberId)
        {
            List<SingleRatingDTORead> sList = new List<SingleRatingDTORead>();
            var rating = await _context.Rating
                .Include(p => p.Isbn)
                .Include(i => i.Isbn.Author).Where(i => i.Membership.Id == memberId).ToListAsync();

            rating.ForEach(r => sList.Add(r.ConvertToSingleDto()));

            if (rating == null)
            {
                return NotFound();
            }

            return sList;
        }


        // GET: api/Ratings/getratingforbook/52423432
        [HttpGet("getratingforbook{isbn}")]
        public async Task<ActionResult<AggregateRatingDTORead>> GetRatingForBook(int isbn)
        {
            var rating = await _context.Rating
                .Include(p => p.Isbn)
                .Include(i => i.Isbn.Author).Where(i => i.Isbn.Isbn == isbn).ToListAsync();
            bool hasRating = rating.Count == 0;
            var dto = new AggregateRatingDTORead()
            {
                AvgRating = hasRating ? rating.Average(p => p.ReaderRating) : 0,
                NoRatings = hasRating ? rating.Count() : 0,
                IsbnDto = _context.ISBNs.Single(i => i.Isbn == isbn).ConvertToDto(),
            };

            if (rating == null)
            {
                return NotFound();
            }

            return dto;
        }

        // PUT: api/Ratings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ratings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            _context.Rating.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
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

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.Id == id);
        }
    }
}
