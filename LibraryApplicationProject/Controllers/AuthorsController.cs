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
            var list = await _context.Authors.Include(person => person.Person).Include(author => author.Isbn).ToListAsync();
            foreach (var auth in list)
            {
                bool pNull = auth.Person == null;

                var authorDto = new AuthorDTORead
                {
                    FirstName = pNull ? "" : auth.Person.FirstName,
                    LastName = pNull ? "" : auth.Person.LastName,
                    BirthDate = pNull ? DateTime.Today : auth.Person.BirthDate,
                    Description = auth.Description,
                    BooksList = auth.Isbn.Select(i => i.ToString()).ToList(),
                };

                authors.Add(authorDto);
            }

            return authors;
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{authorId}, {personId}")]
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

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorDTORead>> PostAuthor(AuthorDTORead authorDtoRead)
        {
            var tuple = authorDtoRead.ConvertFromDto();

            var person = tuple.Item1;
            var author = tuple.Item2;

            _context.Persons.Add(person);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new
            {
                id = author.Id
            }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

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
