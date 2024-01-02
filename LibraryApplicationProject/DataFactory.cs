using LibraryApplicationProject.Controllers;
using LibraryApplicationProject.Data;
using System.Security.Cryptography;
using System.Text;

namespace LibraryApplicationProject
{
    public class DataFactory
    {
        LibraryDbContext _context;

        List<string> _lastNames = new List<string>()
        {
            "Andersson", "Karlsson", "Rayden", "Russel", "Taylor", "Birdie", "Hitchcock", "Penn", "Bacon", "Smith",
            "Kimi", "Clarkson", "Edelblomberg", "Booker", "Crook", "Smoker", "Webber", "Ramsey"
        };
        List<string> _firstNames = new List<string>()
        {
            "Margot", "Astrid", "Charles", "Sean", "Crow", "Welsh", "Tim", "Bob", "Clarence", "Eva", "Lena", "Thomas",
            "Kent", "Sam", "Jonas", "Rikard", "Kalle", "Frank", "Tina", "Albert", "Robert", "Titti", "Hubertius"
        };


        public DataFactory(LibraryDbContext context)
        {
            _context = context;
        }

        private void CreateData()
        {
            Authors(5);
            Memberships(5);
        }


        private void Authors(int noAuthors)
        {
            for (int i = 0; i < noAuthors; i++)
            {
                var auth = new Author
                {
                    Person = CreatePerson(),
                    Description = GenerateLoremIpsum(8),
                };
                _context.Authors.Add(auth);
            }

        }

        private void Memberships(int noMembers)
        {
            var rnd = new Random();
            for(int i = 0;i < noMembers; i++)
            {
                var member = new Membership
                {
                    Person = CreatePerson(),
                    CardNumber = rnd.NextInt64(10000000, 9999999),
                    //RegistryDate = GenerateDate(GeneratedDateVariants.RegistryDate),
                };
            }
        }

        private Person CreatePerson()
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