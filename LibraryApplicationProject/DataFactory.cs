using LibraryApplicationProject.Data;
using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace LibraryApplicationProject
{
    public class DataFactory
    {

        List<string> _lastNames = new List<string>()
        {
            "Andersson", "Karlsson", "Rayden", "Russel", "Taylor", "Birdie", "Hitchcock", "Penn", "Bacon", "Smith",
            "Kimi", "Clarkson", "Edelblomberg", "Booker", "Crook", "Smoker", "Webber", "Ramsey","Lindroos","Råsberg","Edlund",
        };
        List<string> _firstNames = new List<string>()
        {
            "Margot", "Astrid", "Charles", "Sean", "Crow", "Welsh", "Tim", "Bob", "Clarence", "Eva", "Lena", "Thomas",
            "Kent", "Sam", "Jonas", "Rikard", "Kalle", "Frank", "Tina", "Albert", "Robert", "Titti", "Hubertius","Anna","Alex",
        };

        public async Task CreateData(LibraryDbContext _context)
        {
            Authors(_context, 5);
            Memberships(_context, 5);
            await _context.SaveChangesAsync();

        }


        private void Authors(LibraryDbContext _context, int noAuthors)
        {
            for (int i = 0; i < noAuthors; i++)
            {
                var rnd = new Random();
                var person = CreatePerson(_context);
                var auth = new Author
                {
                    Person = person,
                    Description = GenerateLoremIpsum(rnd.Next(2, 8)),
                };
                _context.Authors.Add(auth);

                int writtenBooks = rnd.Next(3);
                for (int j = 0; j < writtenBooks; j++)
                {
                    var copies = rnd.Next(8);
                    var isbn = CreateISBN(_context, auth);
                    CreateBooks(_context, isbn, copies);
                }
            }

        }

        private void Memberships(LibraryDbContext _context, int noMembers)
        {
            var rnd = new Random();
            for (int i = 0; i < noMembers; i++)
            {
                var date = GenerateDate(GeneratedDateVariants.RegistryDate);
                var person = CreatePerson(_context);
                var member = new Membership
                {
                    Person = person,
                    CardNumber = rnd.NextInt64(10000000, 99999999),
                    RegistryDate = date,
                    ExpirationDate = date.AddYears(2),
                };
                _context.Memberships.Add(member);
            }
        }

        private ISBN CreateISBN(LibraryDbContext _context, Author author)
        {
            var rnd = new Random();
            var reDate = author.Person.BirthDate.AddYears(rnd.Next(20, 55));
            var isbn = new ISBN()
            {
                Isbn = rnd.NextInt64(11111111, 99999999),
                Title = GenerateLoremIpsum(rnd.Next(1, 5)),
                Description = GenerateLoremIpsum(rnd.Next(12, 50)),
                ReleaseDate = reDate,
            };
            isbn.Author.Add(author);
            author.Isbn.Add(isbn);
            _context.ISBNs.Add(isbn);
            return isbn;
        }
        private void CreateBooks(LibraryDbContext _context, ISBN isbn, int noBooks)
        {
            for (int i = 0; i < noBooks; i++)
            {
                _context.Books.Add(new Book()
                {
                    Isbn = isbn,
                    IsAvailable = true,
                });
            }
        }

        private Person CreatePerson(LibraryDbContext _context)
        {
            var rnd = new Random();
            var person = new Person
            {
                FirstName = _firstNames[rnd.Next(_firstNames.Count)],
                LastName = _lastNames[rnd.Next(_lastNames.Count)],
                BirthDate = GenerateDate(GeneratedDateVariants.DateOfBirth),
            };
            _context.Persons.Add(person);
            return person;
        }

        #region Helpers
        private static DateOnly GenerateDate(GeneratedDateVariants dateVariant)
        {
            var rnd = new Random();
            var date = dateVariant switch
            {
                GeneratedDateVariants.DateOfBirth => new DateOnly(rnd.Next(1940, 2003), rnd.Next(1, 12), rnd.Next(1, 31)),
                GeneratedDateVariants.RegistryDate => new DateOnly(rnd.Next(2020, 2023), rnd.Next(1, DateTime.Today.Month),
                    rnd.Next(1, DateTime.Today.Day)),
                GeneratedDateVariants.BookingsDate => new DateOnly(2023, DateTime.Now.Month, rnd.Next(1, DateTime.Now.Day)),
                _ => new DateOnly()
            };
            return date;
        }
        private enum GeneratedDateVariants
        {
            DateOfBirth,
            RegistryDate,
            BookingsDate
        }



        private string GenerateLoremIpsum(int numberOfWords)
        {
            if (numberOfWords <= 0)
            {
                throw new ArgumentException("Number of words should be greater than zero.");
            }

            string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. ";
            string[] words = loremIpsum.Split(' ');

            if (numberOfWords <= words.Length)
            {
                // If the requested number of words is less than or equal to the number of words in the base Lorem Ipsum,
                // we can simply take the substring.
                return string.Join(" ", words, 0, numberOfWords);
            }

            // If the requested number of words is more than the base Lorem Ipsum,
            // repeat the Lorem Ipsum until we reach the desired number of words.
            StringBuilder result = new StringBuilder(loremIpsum);
            while (result.ToString().Split(' ').Length < numberOfWords)
            {
                result.Append(loremIpsum);
            }

            // Trim the result to the exact number of words requested.
            string[] resultWords = result.ToString().Split(' ');
            return string.Join(" ", resultWords, 0, numberOfWords);




        }
        #endregion
    }
}