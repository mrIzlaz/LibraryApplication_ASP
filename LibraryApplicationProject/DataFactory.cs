using LibraryApplicationProject.Controllers;
using LibraryApplicationProject.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
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
        List<string> _titles = new List<string>()
        {
            "Sagan om Trolle","Sagan om Ringen", "Sagan om Ringens Återkomst", "Sagan om Ringens Återkomst II",
            "Femte Elementet, en rörmockarers bekännelser","Leif GW Persons Memoarer", "En flaska till", "Skål!",
            "På det skätte small det", "Vem tröstar Bengt?", "Andas ut, ta det lugnt", "Lugna andetag, Yoga-mästarens bekännelse",
            "Vem kastade? Skogsturkens memoarer","Bäst i test, tips och tricks", "Göteborg, känner ingen sorg", "Stockholm utan hjärtan",
            "En natt utan stjärnor", "Urmakarnas krig", "I Trons namn, berättelsen bakom Clu", "Den vita flykten, historien om Romarikets uppkomst och fall",
            "Torkan, Flykten, Sorgen", "Nätter med vänner", "Dagar med Troll", "Järngrinden"
        };

        List<string> taggedTitles = new List<string>();
        public async Task CreateData(LibraryDbContext context)
        {
            Authors(context, 8);
            Memberships(context, 5);
            await context.SaveChangesAsync();
            await Loans(context, 5);
            await Ratings(context, 8);
            await context.SaveChangesAsync();
            await CreateTestUser(context);
        }


        private void Authors(LibraryDbContext context, int authors)
        {
            for (int i = 0; i < authors; i++)
            {
                var rnd = new Random();
                var person = CreatePerson(context);
                var auth = new Author
                {
                    Person = person,
                    Description = GenerateLoremIpsum(rnd.Next(2, 8)),
                };
                context.Authors.Add(auth);

                int writtenBooks = rnd.Next(1, 4);
                for (int j = 0; j < writtenBooks; j++)
                {
                    var copies = rnd.Next(2, 8);
                    var isbn = CreateISBN(context, auth);
                    CreateBooks(context, isbn, copies);
                }
            }

        }

        private void Memberships(LibraryDbContext context, int members)
        {
            var rnd = new Random();
            for (int i = 0; i < members; i++)
            {
                var date = GenerateDate(GeneratedDateVariants.RegistryDate);
                var person = CreatePerson(context);
                var member = new Membership
                {
                    Person = person,
                    CardNumber = rnd.NextInt64(10000000, 99999999),
                    RegistryDate = date,
                    ExpirationDate = date.AddYears(2),
                };
                context.Memberships.Add(member);
            }
        }

        private async Task Loans(LibraryDbContext context, int loans)
        {
            var rnd = new Random();
            var members = await context.Memberships.ToListAsync();
            var books = await context.Books.ToListAsync();
            var noBooks = rnd.Next(1, 5);
            for (int i = 0; i < loans; i++)
            {
                var booklist = new List<Book>();
                for (int j = 0; j < noBooks; j++)
                {
                    var b = books[rnd.Next(books.Count)];
                    int trials = 0;
                    while (!b.IsAvailable)
                    {
                        b = books[rnd.Next(books.Count)];
                        trials++;
                        if (trials > 6)
                        {

                            b = null;
                            break;
                        }
                    }
                    if (b == null) continue;
                    b.IsAvailable = false;
                    booklist.Add(b);
                }
                var date = GenerateDate(GeneratedDateVariants.BookingsDate);
                var loan = new Loan()
                {
                    Membership = members[rnd.Next(members.Count())],
                    Books = booklist,
                    StartDate = date,
                    EndDate = date.AddDays(14),
                    IsActive = true,
                };
                context.Loans.Add(loan);
            }
            return;
        }

        private async Task Ratings(LibraryDbContext context, int ratings)
        {
            var rnd = new Random();
            var members = await context.Memberships.ToListAsync();
            var isbns = await context.ISBNs.ToListAsync();
            for (var i = 0; i < ratings; i++)
            {
                var member = members[rnd.Next(members.Count)];
                for (var j = 0; j < ratings; j++)
                {
                    Rating rating;
                    ISBN isbn;
                    do
                    {
                        isbn = isbns[rnd.Next(isbns.Count)];

                        rating = new Rating()
                        {
                            Membership = member,
                            Isbn = isbn,
                            ReaderRating = rnd.Next(6),
                        };
                    }
                    while (await RatingExists(context, member, isbn));
                    if (rating == null || isbn == null) continue;
                    context.Rating.Add(rating);
                }
            }
        }


        private async Task<bool> RatingExists(LibraryDbContext context, Membership member, ISBN isbn)
        {
            var ratings = await context.Rating.Where(r => r.Membership == member).ToListAsync();
            return ratings.Any(r => r.Isbn == isbn);
        }

        private ISBN CreateISBN(LibraryDbContext context, Author author)
        {
            var rnd = new Random();
            var reDate = author.Person!.BirthDate.AddYears(rnd.Next(20, 55));
            var isbn = new ISBN()
            {
                Isbn = rnd.NextInt64(11111111, 99999999),
                Title = GetTitle(),
                Description = GenerateLoremIpsum(rnd.Next(12, 50)),
                ReleaseDate = reDate,
            };
            isbn.Author.Add(author);
            author.Isbn.Add(isbn);
            context.ISBNs.Add(isbn);
            return isbn;
        }

        private string GetTitle()
        {
            var rnd = new Random();
            var title = string.Empty;
            do
            {
                title = _titles[rnd.Next(_titles.Count)];
            } while (taggedTitles.Contains(title));
            taggedTitles.Add(title);
            return title;
        }
        private void CreateBooks(LibraryDbContext context, ISBN isbn, int noBooks)
        {
            for (int i = 0; i < noBooks; i++)
            {
                context.Books.Add(new Book()
                {
                    Isbn = isbn,
                    IsAvailable = true,
                });
            }
        }
        private Person CreatePerson(LibraryDbContext context)
        {
            var rnd = new Random();
            var person = new Person
            {
                FirstName = _firstNames[rnd.Next(_firstNames.Count)],
                LastName = _lastNames[rnd.Next(_lastNames.Count)],
                BirthDate = GenerateDate(GeneratedDateVariants.DateOfBirth),
            };
            context.Persons.Add(person);
            return person;
        }
        private async Task CreateTestUser(LibraryDbContext context)
        {
            var person = new Person()
            {
                FirstName = "Test",
                LastName = "Tester",
                BirthDate = new DateOnly(1989, 08, 14),
            };
            context.Persons.Add(person);
            await context.SaveChangesAsync();
            var member = new Membership
            {
                Person = person,
                CardNumber = -1,
                RegistryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddYears(8)),
            };
            context.Memberships.Add(member);
            await context.SaveChangesAsync();
        }

        #region Helpers
        private static DateOnly GenerateDate(GeneratedDateVariants dateVariant)
        {
            var rnd = new Random();
            var date = dateVariant switch
            {
                GeneratedDateVariants.DateOfBirth => new DateOnly(rnd.Next(1940, 2003), rnd.Next(1, 12), rnd.Next(1, 28)),
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