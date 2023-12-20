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
using Humanizer;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO dto)
        {
            var authList = new List<Author>();

            foreach (var i in dto.AuthorId)
            {
                var auth = await _context.Authors.FindAsync(i);
                if (auth != null)
                    authList.Add(auth);
            }

            ISBN isbn = new ISBN()
            {
                Isbn_Id = dto.Id,
                Isbn = dto.Isbn,
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Author = authList,
            };
            Book book = new();
            for (int i = 0; i < dto.Quantity; i++)
            {
                book = new Book()
                {
                    IsAvailable = true,
                    Isbn = isbn
                };

                _context.Books.Add(book);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction($"GetBook, quantity: {dto.Quantity}", new { id = dto.Id }, book);
        }
        [HttpPost("{dto}")]
        public async Task<ActionResult<AuthorBookDTO>> PostBookWithAuthor(AuthorBookDTO dto)
        {
            var authList = new List<Author>();

            foreach (var i in dto.Authors)
            {
                authList.Add(new Author
                {
                   Description = i.Description,
                   Person = new Person
                   {
                       FirstName = i.FirstName,
                       LastName = i.LastName,
                       BirthDate = i.BirthDate
                   },
                });
            }

            ISBN isbn = new ISBN()
            {
                Isbn_Id = dto.Id,
                Isbn = dto.Isbn,
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Author = authList,
            };
            Book book = new();
            for (int i = 0; i < dto.Quantity; i++)
            {
                book = new Book()
                {
                    IsAvailable = true,
                    Isbn = isbn
                };

                _context.Books.Add(book);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction($"GetBook, quantity: {dto.Quantity}", new { id = dto.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
