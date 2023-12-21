using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApplicationProject;
using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using Humanizer;
using LibraryApplicationProject.Data.Extension;

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
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            List<BookDTO> getBooks = new List<BookDTO>();
            var books = await _context.Books.Include(i => i.Isbn)
                .Include(a => a.Isbn.Author)
                .ThenInclude(a => a.Person)
                .ToListAsync();

            foreach (var book in books)
            {
                var dto = book.ConvertToDto();
                getBooks.Add(dto);
            }

            return getBooks;
        }

        // GET: api/Books/5
        [HttpGet("{isbn}")]
        public async Task<ActionResult<BookSearchDTO>> GetBookByISBN(long isbn)
        {
            //var book = await _context.Books.FindAsync(id);
            var books = await _context.Books
                .Include(book => book.Isbn)
                .Include(a => a.Isbn.Author)
                .ThenInclude(a => a.Person)
                .ToListAsync();
            var book = books.First(b => b.Isbn.Isbn == isbn);
            if (book == null)
            {
                return NotFound();
            }
            var dto = book.ConvertToDto();
            var quantity = books.Count(b => b.Isbn.Isbn == isbn);
            var available = books.Count(b => b.Isbn.Isbn == isbn && b.IsAvailable);
            var result = new BookSearchDTO
            {
                Isbn = isbn,
                Quantity = quantity,
                Available = available,
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Authors = dto.Authors,
            };

            return result;
        }


        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{isbn}")]
        public async Task<IActionResult> PutBook(int isbn, BookEntryDTO dto)
        {
            if (isbn != dto.Isbn)
            {
                return BadRequest();
            }

            try
            {
                var isbnVal = _context.ISBNs.Single(i => i.Isbn == isbn);
                var book = dto.ConvertFromDto(isbnVal);

                var diff = dto.Quantity - _context.Books.Count(b => b.Isbn.Isbn == isbn);

                if (diff > 0)
                {
                    BookFactory(diff, isbnVal);

                }
                else if (diff < 0)
                {
                    //Remove diff
                }


                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!BookExistsISBN(isbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookEntryDTO>> PostBook(BookEntryDTO entryDto)
        {
            var authList = new List<Author>();

            foreach (var i in entryDto.Authors)
            {
                var auth = await _context.Authors.FindAsync(i);
                if (auth != null)
                    authList.Add(auth);
            }

            ISBN isbn = new ISBN()
            {
                Isbn_Id = entryDto.Id,
                Isbn = entryDto.Isbn,
                Title = entryDto.Title,
                Description = entryDto.Description,
                ReleaseDate = entryDto.ReleaseDate,
                Author = authList,
            };
            Book book = new();
            for (int i = 0; i < entryDto.Quantity; i++)
            {
                book = new Book()
                {
                    IsAvailable = true,
                    Isbn = isbn
                };

                _context.Books.Add(book);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction($"GetBook, quantity: {entryDto.Quantity}", new { id = entryDto.Id }, book);
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
            var book = BookFactory(dto.Quantity, isbn);
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

        private bool BookExistsISBN(int isbn)
        {
            return _context.Books.Any(e => e.Isbn.Isbn == isbn);
        }

        private Book BookFactory(int i, ISBN isbn)
        {
            Book book = new();
            for (int ii = 0; ii < i; ii++)
            {
                book = new Book()
                {
                    IsAvailable = true,
                    Isbn = isbn
                };

                _context.Books.Add(book);
            }

            return book;
        }

    }
}
