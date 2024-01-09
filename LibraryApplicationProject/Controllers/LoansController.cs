using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public LoansController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDTORead>>> GetLoans()
        {
            return await _context.Loans
                .Include(m => m.Membership!.Person)
                .Include(b => b.Books).ThenInclude(a => a.Isbn!.Author)
                .ThenInclude(p => p.Person)
                .AsNoTracking()
                .Select(x => x.ConvertToDto()).ToListAsync();
        }

        // GET: api/Loans/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // PUT: api/Loans/return/5

        [HttpPut("return/{bookId:int}")]
        public async Task<ActionResult> PutLoan(int bookId, int? rating)
        {
            var book = await _context.Books
                .Include(b => b.Isbn)
                .SingleAsync(b => b.Id == bookId);

            var loan = await _context.Loans
                .Include(loan => loan.Books)
                .ThenInclude(b => b.Isbn)
                .ThenInclude(i => i!.Author)
                .ThenInclude(a => a.Person)
                .Include(m => m.Membership)
                .Include(m => m.Membership!.Person)
                .SingleAsync(l => l.IsActive && l.Books.Contains(book));

            if (rating != null)
            {
                if (rating is > 5 or < 0)
                    return BadRequest("Rating must be between 0 and 5");
                _context.Rating.Add(new Rating()
                {
                    Isbn = book.Isbn,
                    Membership = loan.Membership,
                    ReaderRating = (int)rating,
                });
            }
            book.IsAvailable = true;
            if (loan.Books.All(b => b.IsAvailable))
                loan.CloseLoan();

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loan.Id))
                {
                    return NotFound();
                }

                throw;
            }
            if (loan.IsActive)
                return NoContent();
            return CreatedAtAction("CloseLoan", new { id = loan.Id }, loan);

        }


        // PUT: api/Loans/close/5

        [HttpPut("close/{id:int}")]
        public async Task<ActionResult<LoanDTORead>> CloseLoan(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Books)
                .ThenInclude(b => b.Isbn)
                .ThenInclude(i => i!.Author)
                .Include(l => l.Membership)
                .Include(l => l.Membership!.Person)
                .SingleAsync(l => l.Id == id);

            if (id != loan.Id) return BadRequest();

            loan.CloseLoan();
            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                    return NotFound();
                else
                    throw;
            }

            var dto = loan.ConvertToDto();

            return dto;
        }

        // POST: api/Loans/new
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        public async Task<ActionResult<LoanDTORead>> PostLoan(LoanDTOEntry dtoEntry)
        {
            Loan loan;
            try
            {
                var member = _context.Memberships
                    .Include(p => p.Person)
                    .Single(m => m.CardNumber == dtoEntry.MembershipCardNumber);

                if (!member.IsStillValid(DateOnly.FromDateTime(DateTime.Today)))
                    return Problem("Membership expired");
                if (dtoEntry.BookIds.IsNullOrEmpty())
                    return BadRequest("No books to loan");

                var allBooks = await _context.Books
                    .Include(i => i.Isbn)
                    .Include(a => a.Isbn!.Author)
                    .ThenInclude(p => p.Person)
                    .ToListAsync();

                var books = new List<Book>();
                foreach (var bookId in dtoEntry.BookIds)
                {
                    var book = allBooks.Single(b => b.Id == bookId);
                    if (!book.IsAvailable)
                        return Conflict($"bookId: {bookId} {book.Isbn?.Title} is not Available");
                    books.Add(book);
                    book.IsAvailable = false;
                }

                loan = dtoEntry.ConvertFromDto(member, books);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            var loanRead = loan.ConvertToDto();

            return CreatedAtAction("GetLoan", new { id = loan.Id }, loanRead);
        }
        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
