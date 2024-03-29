﻿using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        // GET: api/Books/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BookDTORead>>> GetBooks()
        {
            List<BookDTORead> getBooks = new List<BookDTORead>();
            var books = await _context.Books.Include(i => i.Isbn)
                .Include(a => a.Isbn!.Author)
                .ThenInclude(a => a.Person)
                .AsNoTracking()
                .ToListAsync();

            var ratings = new Dictionary<long, double>();
            var distinctIsbn = books.DistinctBy(b => b.Isbn!.Isbn);
            foreach (var book in distinctIsbn)
            {
                if (book.Isbn == null) continue;
                var isbn = book.Isbn.Isbn;
                var rating = await GetBookRating(isbn);
                ratings.Add(isbn, rating);
            }

            foreach (var book in books)
            {
                ratings.TryGetValue(book.Isbn.Isbn, out var rating);
                var dto = book.ConvertToDtoRead(rating);
                getBooks.Add(dto);
            }

            return getBooks;
        }
        // GET: api/Books/stock
        [HttpGet("stock")]
        public async Task<ActionResult<IEnumerable<BookSearchDTO>>> GetBooksOverview()
        {
            List<BookSearchDTO> getBooks = new List<BookSearchDTO>();
            var books = await _context.Books.Include(i => i.Isbn)
                .Include(a => a.Isbn!.Author)
                .ThenInclude(a => a.Person)
                .AsNoTracking()
                .ToListAsync();

            books.DistinctBy(b => b.Isbn!.Isbn).ToList().ForEach(x => getBooks.Add(x.ConvertToDtoSearch()));
            foreach (var book in getBooks)
            {
                var length = book.Description.Length;
                if (book.Description.Length > 50)
                    length = 50;
                book.Description = book.Description[..length] + "...";
                var rating = await GetBookRating(book.Isbn);
                book.AvgRating = rating;
                var tuple = await GetBookStock(book.Isbn);
                book.Available = tuple.Available;
                book.Quantity = tuple.Quantity;
            }

            return getBooks;
        }

        // GET: api/Books/stock/1234567890
        [HttpGet("stock/{isbn:long}")]
        public async Task<ActionResult<BookSearchDTO>> GetBookByISBN(long isbn)
        {
            var books = await _context.Books
                .Include(book => book.Isbn)
                .Include(a => a.Isbn!.Author)
                .ThenInclude(a => a.Person)
                .AsNoTracking()
                .ToListAsync();
            var book = books.First(b => b.Isbn != null && b.Isbn.Isbn == isbn);
            var dto = book.ConvertToDtoSearch();

            var tup = await GetBookStock(dto.Isbn);
            dto.Available = tup.Available;
            dto.Quantity = tup.Quantity;
            dto.AvgRating = await GetBookRating(isbn);
            return dto;
        }

        //Get: api/Books/info/2
        [HttpGet("info/{id:int}")]
        public async Task<ActionResult<BookDTORead>> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.Isbn)
                .ThenInclude(i => i.Author)
                .ThenInclude(a => a.Person)
                .AsNoTracking()
                .SingleAsync(b => b.Id == id);

            BookDTORead dto = book.ConvertToDtoRead();

            LoanDTORead? loanDto = null;
            if (!book.IsAvailable)
            {
                var loan = await _context.Loans
                    .Include(l => l.Membership)
                    .ThenInclude(l => l.Person)
                    .AsNoTracking()
                    .AsNoTracking()
                    .SingleAsync(l => l.IsActive && l.Books.Contains(book));
                loanDto = loan.ConvertToDto();
            }


            var response = new
            {
                BookData = dto,
                Loan = loanDto,
            };

            return Ok(response);
        }
        // PUT: api/Books/update/1234567890
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{isbn:long}")]
        public async Task<IActionResult> PutBook(long isbn, BookEntryDTO dto)
        {
            if (isbn != dto.Isbn)
            {
                return BadRequest();
            }
            try
            {
                var isbnVal = _context.ISBNs.Single(i => i.Isbn == isbn);
                var book = dto.ConvertFromDto(isbnVal);

                var diff = dto.Quantity - _context.Books.Count(b => b.Isbn != null && b.Isbn.Isbn == isbn);

                if (diff > 0)
                {
                    BookFactory(diff, isbnVal);
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

        // PATCH: api/Books/update/1234567890/author/2
        [HttpPatch("update/{isbn:long}/author/{authorId:int}")]
        public async Task<IActionResult> AddAuthorToExistingISBN(long isbn, int authorId)
        {

            if (isbn <= 0 || authorId <= 0)
            {
                return BadRequest("Invalid ISBN or AuthorId.");
            }

            if (!BookExistsISBN(isbn) || !AuthorExists(authorId))
            {
                return NotFound();
            }

            var isbnVal = await _context.ISBNs.SingleAsync(i => i.Isbn == isbn);
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null)
            {
                return BadRequest("Invalid authorId.");
            }

            isbnVal.Author.Add(author);
            _context.Entry(isbnVal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();

        }
        // POST: api/Books/new
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        public async Task<ActionResult<BookEntryDTO>> PostBook(BookEntryDTO entryDto)
        {
            var authList = new List<Author>();

            foreach (var i in entryDto.Authors)
            {
                var auth = await _context.Authors.FindAsync(i);
                if (auth != null)
                    authList.Add(auth);
            }

            var isbn = entryDto.ConvertFromDto(authList);
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

            return CreatedAtAction("GetBookByISBN", new { isbn = entryDto.Isbn }, entryDto);

        }
        // POST: api/Books/new/author
        [HttpPost("new/author")]
        public async Task<ActionResult<AuthorBookDTO>> PostBookWithAuthor(AuthorBookDTO dto)
        {
            var isbn = dto.ConvertFromDto(dto.Authors.ConvertFromDtoList());
            var books = BookFactory(dto.Quantity, isbn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookByISBN", new { isbn = dto.Isbn }, dto);

        }

        // DELETE: api/Books/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                return BadRequest("Book needs to be available/returned to be able to be deleted");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Books/isbn/5312313123
        [HttpDelete("isbn/{isbn:long}")]
        public async Task<IActionResult> DeleteBook(long isbn)
        {
            var isbnVal = await _context.ISBNs.SingleAsync(i => i.Isbn == isbn);

            var books = await _context.Books
                .Include(b => b.Isbn)
                .Where(b => b.Isbn != null && b.Isbn.Isbn == isbn).ToListAsync();

            // Check if there are books in database
            if (!books.IsNullOrEmpty())
            {
                return BadRequest("Physical books needs to be removed from database for ISBN to be removed properly");
            }

            _context.Authors.Where(a => a.Isbn.Contains(isbnVal)).ToList().ForEach(a =>
            {
                a.Isbn.Remove(isbnVal);
                _context.Entry(a).State = EntityState.Modified;
            });
            _context.ISBNs.Remove(isbnVal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Books/isbn/5312313123
        [HttpDelete("remove/isbn/{isbn:long}")]
        public async Task<IActionResult> DeleteBooksAssociated(long isbn)
        {
            var isbnVal = await _context.ISBNs.SingleAsync(i => i.Isbn == isbn);

            var books = await _context.Books
                .Include(b => b.Isbn)
                .Where(b => b.Isbn != null && b.Isbn.Isbn == isbn).ToListAsync();

            // Check if there are books in database
            foreach (var book in books)
            {
                book.Isbn = null;
                _context.Remove(book);
            }

            var ratings = await _context.Rating.Where(r => r.Isbn == isbnVal).ToListAsync();

            foreach (var rating in ratings)
            {
                rating.Isbn = null;
                _context.Remove(rating);
            }

            _context.Authors.Where(a => a.Isbn.Contains(isbnVal)).ToList().ForEach(a =>
            {
                a.Isbn.Remove(isbnVal);
                _context.Entry(a).State = EntityState.Modified;
            });
            _context.ISBNs.Remove(isbnVal);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool BookExists(int id) => _context.Books.Any(e => e.Id == id);
        private bool BookExistsISBN(long isbn) => _context.Books.Include(i => i.Isbn).Any(e => e.Isbn != null && e.Isbn.Isbn == isbn);
        private bool AuthorExists(int id) => _context.Authors.Any(a => a.Id == id);

        private async Task<(int Quantity, int Available)> GetBookStock(long isbn)
        {
            var books = await _context.Books.Where(b => b.Isbn != null && b.Isbn.Isbn == isbn)
                .Include(book => book.Isbn).ToListAsync();
            var quantity = books.Count(b => b.Isbn != null && b.Isbn.Isbn == isbn);
            var available = books.Count(b => b.Isbn != null && b.Isbn.Isbn == isbn && b.IsAvailable);

            return (quantity, available);
        }

        private async Task<double> GetBookRating(long isbn)
        {
            try
            {
                var ratings = await _context.Rating
                    .Where(r => r.Isbn != null && r.Isbn.Isbn == isbn)
                    .ToListAsync();

                if (ratings.Any())
                {
                    var averageRating = ratings.Average(r => r.ReaderRating);
                    return Math.Round(averageRating, 2);
                }
                else
                {
                    // No ratings found
                    return 0; // or another suitable default value
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with: {isbn}. Message: {ex}");
                return -1; // or handle the exception in a way that makes sense for your application
            }
        }
        private List<Book> BookFactory(int i, ISBN isbn)
        {
            var list = new List<Book>();
            Book book = new();
            for (int ii = 0; ii < i; ii++)
            {
                book = new Book()
                {
                    IsAvailable = true,
                    Isbn = isbn
                };

                _context.Books.Add(book);
                list.Add(book);
            }

            return list;
        }

    }
}
