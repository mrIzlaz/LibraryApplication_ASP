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
using Microsoft.IdentityModel.Tokens;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ISBNsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ISBNsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/ISBNs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ISBN>>> GetISBNs()
        {
            return await _context.ISBNs.ToListAsync();
        }

        // GET: api/ISBNs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISBNDTO>> GetISBN(long id)
        {
            var ISBN = await _context.ISBNs.FindAsync(id);
            if (ISBN == null)
            {
                return NotFound();
            }
            List<string?> authList = new();
            if (!ISBN.Author.IsNullOrEmpty())
                authList = ISBN.Author.ConvertToStrings();

            var dto = new ISBNDTO()
            {
                Id = ISBN.Isbn_Id,
                Isbn = ISBN.Isbn,
                Title = ISBN.Title,
                Description = ISBN.Description,
                ReleaseDate = ISBN.ReleaseDate,
                Authors = authList
            };

            return dto;
        }

        // PUT: api/ISBNs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutISBN(long id, ISBN iSBN)
        {
            if (id != iSBN.Isbn_Id)
            {
                return BadRequest();
            }

            _context.Entry(iSBN).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ISBNExists(id))
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

        // POST: api/ISBNs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ISBN>> PostISBN(ISBNDTO DTO)
        {
            if (ISBNExists(DTO.Id))
                return Problem("Isbn already exists in database");

            var authList = new List<Author>();

            foreach (var i in DTO.Authors)
            {
                var auth = await _context.Authors.FindAsync(i);
                if (auth != null)
                    authList.Add(auth);
            }

            var ISBN = new ISBN
            {
                Isbn_Id = DTO.Id,
                Isbn = DTO.Isbn,
                Title = DTO.Title,
                Description = DTO.Description,
                ReleaseDate = DTO.ReleaseDate,
                Author = authList,
            };


            _context.ISBNs.Add(ISBN);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetISBN", new { id = DTO.Id }, DTO);
        }

        // DELETE: api/ISBNs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteISBN(int id)
        {
            var iSBN = await _context.ISBNs.FindAsync(id);
            if (iSBN == null)
            {
                return NotFound();
            }

            _context.ISBNs.Remove(iSBN);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ISBNExists(long id)
        {
            return _context.ISBNs.Any(e => e.Isbn_Id == id);
        }
    }
}
