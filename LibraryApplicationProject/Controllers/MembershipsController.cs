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
        public async Task<ActionResult<IEnumerable<Membership>>> GetMemberships()
        {
            return await _context.Memberships.Include(person => person.Person).ToListAsync();
        }

        // GET: api/Memberships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipDTO>> GetMembership(int id)
        {
            var membership = await _context.Memberships.Include(p => p.Person).FirstAsync(m => m.Id == id);
            if (membership == null) return NotFound();
            if (membership.Person == null) return NotFound();

            var member = await _context.Persons.FindAsync(membership.Person.Id);

            if (member == null) return NotFound();

            var dto = new MembershipDTO
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate,
                RegistryDate = membership.RegistryDate,
                ExpirationDate = membership.ExpirationDate
            };
            return dto;
        }

        // POST: api/Memberships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MembershipDTO>> PostMembership(MembershipDTO membershipDto)
        {

            try
            {
                var person = new Person
                {
                    LastName = membershipDto.LastName,
                    FirstName = membershipDto.FirstName,
                    BirthDate = membershipDto.BirthDate
                };
                membershipDto.RegistryDate = membershipDto.RegistryDate <= DateTime.Today ? DateTime.Now : membershipDto.RegistryDate;
                Membership membership = new Membership
                {
                    RegistryDate = membershipDto.RegistryDate,
                    ExpirationDate = membershipDto.ExpirationDate,
                    Person = person
                };

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

        // DELETE: api/Memberships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.Id == id);
        }
        private bool MembershipExists(Person person)
        {
            return _context.Memberships.Any(e => e.Person != null && e.Person.Id == person.Id);
        }
    }
}
