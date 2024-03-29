﻿using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTORead>>> GetAuthors()
        {
            List<AuthorDTORead> authors = new List<AuthorDTORead>();
            var list = await _context.Authors
                .Include(person => person.Person)
                .Include(author => author.Isbn)
                .AsNoTracking()
                .ToListAsync();
            list.ForEach(a => authors.Add(a.ConvertToDto()));
            return authors;
        }

        // GET: api/Authors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorDTORead>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(p => p.Person)
                .Include(a => a.Isbn)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author.ConvertToDto();
        }

        // PUT: api/Authors/person/5/author/2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("person/{personId:int}/author/{authorId:int}")]
        public async Task<IActionResult> PutAuthor(int authorId, int personId)
        {
            if (!AuthorExists(authorId))
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(authorId);
            var person = await _context.Persons.FindAsync(personId);

            if (person == null || author == null)
                return NotFound();
            author.Person = person;
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(authorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var dto = author.ConvertToDto();
            return CreatedAtAction("GetAuthor", new { id = author.Id }, dto);
        }

        // POST: api/Authors/new
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        public async Task<ActionResult<AuthorDTORead>> PostAuthor(AuthorDTOInsert authorDtoRead)
        {
            var tuple = authorDtoRead.ConvertFromDto();

            var person = tuple.Item1;
            var author = tuple.Item2;

            _context.Persons.Add(person);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Person)
                .Include(a => a.Isbn)
                .SingleAsync(a => a.Id == id);

            if (author.Person != null) // If person is not null. Check if it's also a member. If not, remove associated person, otherwise throw a Conflict Message.
            {
                var membership = await _context.Memberships
                    .Include(m => m.Person)
                    .FirstOrDefaultAsync(m => m.Person != null && m.Person.Id == author.Person.Id);
                if (membership == null) //Is Author also a Member?
                    _context.Persons.Remove(author.Person);
                else
                    return Conflict($"Author is also a Member with membershipId: {membership.Id}" +
                                    $", please also remove the Membership to fully remove associated data");
            }
            _context.ISBNs.Where(i => i.Author.Contains(author)).ToList().ForEach(i =>
            {
                i.Author.Remove(author);
                _context.Entry(i).State = EntityState.Modified;
            }); //Remove all references to Author

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
