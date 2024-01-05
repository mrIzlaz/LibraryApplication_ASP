using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApplicationProject.Data;
using LibraryApplicationProject.Data.DTO;
using LibraryApplicationProject.Data.Extension;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace LibraryApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public MembershipsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Memberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipDTO>>> GetMemberships()
        {
            List<MembershipDTO> list = new List<MembershipDTO>();

            var memberships = await _context.Memberships.Include(person => person.Person).ToListAsync();
            var activeLoans = await _context.Loans.Where(l => l.IsActive).ToListAsync();
            memberships.ForEach(x => list.Add(x.ConvertToDto()));
            foreach (var loan in activeLoans)
            {
                if (loan.Membership == null) continue;
                list.Single(m => m.MembershipId == loan.Membership.Id).HasActiveLoan = true;
            }
            return list;
        }

        // GET: api/Memberships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipDTO>> GetMembership(int id)
        {
            var membership = await _context.Memberships
                .Include(p => p.Person)
                .FirstAsync(m => m.Id == id);

            if (membership.Person == null) return NotFound();

            var person = await _context.Persons.FindAsync(membership.Person.Id);

            if (person == null) return NotFound();

            var hasActiveLoan = await MembershipHasActiveLoan(membership.Id);
            var dto = membership.ConvertToDto(hasActiveLoan);
            return dto;
        }

        [HttpPut]
        public async Task<IActionResult> PutMembership(int id, MembershipDTO dto)
        {
            var membership = await _context.Memberships.Include(m => m.Person).FirstOrDefaultAsync(m => m.Id == id);
            if (membership == null) return NotFound("Membership not found");

            var tuple = dto.ConvertFromDto();

            // Detach the existing membership from the context
            _context.Entry(membership).State = EntityState.Detached;

            var person = tuple.Item1;

            // Find the existing person in the context
            var existingPerson = await _context.Persons.FirstOrDefaultAsync(p => membership.Person != null && p.Id == membership.Person.Id);

            // Detach the existing person from the context
            _context.Entry(existingPerson).State = EntityState.Detached;

            if (existingPerson == null)
            {
                // Create new person if person not found.
                _context.Persons.Add(person);
            }
            else
            {
                // Update existing person with new information
                person.Id = existingPerson.Id;
                _context.Entry(existingPerson).CurrentValues.SetValues(person);
            }

            tuple.Item2.Id = membership.Id;
            membership = tuple.Item2;
            membership.Person = person;
            _context.Entry(membership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipExists(id))
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


        // POST: api/Memberships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MembershipDTO>> PostMembership(MembershipDTO membershipDto)
        {

            try
            {
                var tup = membershipDto.ConvertFromDto();

                var person = tup.Item1;
                var membership = tup.Item2;

                if (MembershipExists(person))
                    return Conflict();
                _context.Persons.Add(person);
                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMembership", new { id = membership.Id }, membership);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            var membership = await _context.Memberships
                .Include(m => m.Person)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membership == null)
            {
                return NotFound();
            }

            // Check for active loans
            if (_context.Loans.Any(l => l.IsActive && l.Membership != null && l.Membership.Id == id))
                return BadRequest("Membership has active Loans");

            // Mark related Ratings as null
            _context.Rating
                .Where(r => r.Membership != null && r.Membership.Id == id)
                .ToList()
                .ForEach(x => x.Membership = null);

            // Deletes related Loans
            var loans = _context.Loans
                .Include(loan => loan.Books)
                .Where(l => l.Membership != null && l.Membership.Id == id).ToList();
            foreach (var loan in loans)
            {
                // Set Books' Loan navigation property to null
                foreach (var book in loan.Books)
                {
                    // Assuming you have an IsAvailable property on the Book class
                    book.IsAvailable = true;
                    _context.Entry(book).State = EntityState.Modified;
                }
                loan.Books.Clear();
                _context.Remove(loan);
            }

            await _context.SaveChangesAsync();

            // Remove the membership
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();

            // Check if the person is an author and handle accordingly
            if (_context.Authors.Any(a => a.Person != null && a.Person.Id == membership.Person.Id))
                return Conflict("Membership holder is an Author, removing only membership");

            // Remove the person (if not used in other relationships)
            if (membership.Person != null)
            {
                _context.Persons.Remove(membership.Person);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool MembershipExists(int id) => _context.Memberships.Any(e => e.Id == id);
        private bool MembershipExists(Person person) => _context.Memberships.Any(e => e.Person != null && e.Person.Id == person.Id);
        private Task<bool> MembershipHasActiveLoan(int memberId) =>
            _context.Loans.Include(l => l.Membership).AnyAsync(l => l.IsActive && l.Membership != null && l.Membership.Id == memberId);

    }
}
