using LibraryApplicationProject.Data.DTO;
using System;
namespace LibraryApplicationProject.Data.Extension;

public static class DTOExtension
{
    #region Book
    public static BookDTO ConvertToDto(this Book book)
    {
        book.Isbn ??= new ISBN
        {
            Isbn_Id = 0,
            Isbn = 0,
            Title = "null",
            Description = "null",
            ReleaseDate = default,
            Author = new List<Author>(),
        };
        return new BookDTO
        {
            IsAvailable = book.IsAvailable,
            Isbn = book.Isbn.Isbn,
            Title = book.Isbn.Title,
            Description = book.Isbn.Description,
            ReleaseDate = book.Isbn.ReleaseDate,
            Authors = book.Isbn.Author.ConvertToIds(),
        };
    }
    public static BookSearchDTO ConvertToDtoSearch(this Book book)
    {
        book.Isbn ??= new ISBN
        {
            Isbn_Id = 0,
            Isbn = 0,
            Title = "null",
            Description = "null",
            ReleaseDate = default,
            Author = new List<Author>(),
        };
        return new BookSearchDTO
        {
            Isbn = book.Isbn.Isbn,
            Title = book.Isbn.Title,
            Description = book.Isbn.Description,
            ReleaseDate = book.Isbn.ReleaseDate,
            Authors = book.Isbn.Author.ConvertToStrings(),
        };
    }
    public static BookDTORead ConvertToDtoRead(this Book book)
    {
        return book.ConvertToDtoRead(0);
    }

    public static BookDTORead ConvertToDtoRead(this Book book, double rating)
    {
        book.Isbn ??= new ISBN
        {
            Isbn_Id = 0,
            Isbn = 0,
            Title = "null",
            Description = "null",
            ReleaseDate = default,
            Author = new List<Author>(),
        };
        return new BookDTORead
        {
            Id = book.Id,
            AvgRating = rating,
            IsAvailable = book.IsAvailable,
            Isbn = book.Isbn.Isbn,
            Title = book.Isbn.Title,
            Description = book.Isbn.Description,
            ReleaseDate = book.Isbn.ReleaseDate,
            Authors = book.Isbn.Author.ConvertToStrings(),
        };
    }


    public static ISBN ConvertFromDto(this BookEntryDTO entryDto, List<Author> authList)
    {
        return new ISBN()
        {
            Isbn = entryDto.Isbn,
            Title = entryDto.Title,
            Description = entryDto.Description,
            ReleaseDate = entryDto.ReleaseDate,
            Author = authList,
        };
    }
    public static Book ConvertFromDto(this BookEntryDTO dto, ISBN isbn)
    {
        var book = new Book
        {
            Isbn = isbn,
            IsAvailable = true
        };
        return book;
    }

    #endregion

    #region Author
    public static (Person, Author) ConvertFromDto(this AuthorDTOInsert dtoRead)
    {
        var person = new Person
        {
            FirstName = dtoRead.FirstName,
            LastName = dtoRead.LastName,
            BirthDate = dtoRead.BirthDate
        };
        var author = new Author
        {
            Description = dtoRead.Description,
            Person = person,
        };
        return (person, author);
    }

    public static AuthorDTORead ConvertToDto(this Author author)
    {
        bool pNull = author.Person == null;
        return new AuthorDTORead
        {
            Id = author.Id,
            FirstName = pNull ? "" : author.Person!.FirstName,
            LastName = pNull ? "" : author.Person!.LastName,
            BirthDate = pNull ? DateOnly.FromDateTime(DateTime.Today) : author.Person!.BirthDate,
            Description = author.Description,
            BooksList = author.Isbn.Select(i => i.ToString()).ToList(),
        };
    }
    public static List<Author> ConvertFromDtoList(this List<AuthorDTOInsert> list)
    {
        var authList = new List<Author>();

        foreach (var i in list)
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
        return authList;
    }
    #endregion

    #region Ratings

    public static SingleRatingDTORead ConvertToSingleDto(this Rating rating)
    {
        return new SingleRatingDTORead()
        {
            Isbn = rating.Isbn == null ? -1 : rating.Isbn.Isbn,
            MembershipId = rating.Membership == null ? -1 : rating.Membership.Id,
            Rating = rating.ReaderRating
        };
    }

    public static List<SingleRatingDTORead> ConvertToSingleDtoList(this List<Rating> ratings)
    {
        List<SingleRatingDTORead> list = new List<SingleRatingDTORead>();
        foreach (var rating in ratings)
        {
            list.Add(rating.ConvertToSingleDto());
        }
        return list;
    }

    #endregion

    #region Membership

    public static (Person, Membership) ConvertFromDto(this MembershipDTOPost dto)
    {
        var person = new Person
        {
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            BirthDate = dto.BirthDate
        };
        dto.RegistryDate = dto.RegistryDate <= DateOnly.FromDateTime(DateTime.Today) ? DateOnly.FromDateTime(DateTime.Today) : dto.RegistryDate;
        Membership membership = new Membership
        {
            CardNumber = dto.CardNumber,
            RegistryDate = dto.RegistryDate,
            ExpirationDate = dto.ExpirationDate,
            Person = person
        };
        return (person, membership);
    }

    public static MembershipDTOPost ConvertToDto(this Membership membership)
    {
        var person = membership.Person ?? new Person();
        return membership.ConvertToDto(false);
    }

    public static MembershipDTORead ConvertToDtoRead(this Membership membership)
    {
        var person = membership.Person ?? new Person();
        return new MembershipDTORead
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            CardNumber = membership.CardNumber,
        };
    }

    public static MembershipDTOPost ConvertToDto(this Membership membership, bool hasActiveLoans)
    {
        var person = membership.Person ?? new Person();
        return new MembershipDTOPost
        {

            MembershipId = membership.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            CardNumber = membership.CardNumber,
            RegistryDate = membership.RegistryDate,
            ExpirationDate = membership.ExpirationDate,
            HasActiveLoan = hasActiveLoans,
        };
    }


    #endregion

    #region Loan

    public static LoanDTORead ConvertToDto(this Loan loan)
    {
        var books = new List<BookDTORead>();
        loan.Books.ForEach(x => books.Add(x.ConvertToDtoRead()));
        loan.Membership ??= new Membership
        {
            Id = 0,
            CardNumber = -1,
            RegistryDate = default,
            ExpirationDate = null,
            Person = new Person
            {
                Id = 0,
                FirstName = "null",
                LastName = "null",
                BirthDate = default
            }
        };

        return new LoanDTORead
        {
            Id = loan.Id,
            IsActive = loan.IsActive,
            MembershipCardNumber = loan.Membership.CardNumber,
            FirstName = loan.Membership.Person!.FirstName,
            LastName = loan.Membership.Person.LastName,
            StartDate = loan.StartDate,
            ReturnDate = loan.EndDate,
            Books = books,
        };
    }

    public static Loan ConvertFromDto(this LoanDTOEntry loanDtoEntry, Membership member, List<Book> books)
    {

        return new Loan
        {
            Membership = member,
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = loanDtoEntry.ReturnDate,
            IsActive = true,
            Books = books,
        };
    }

    #endregion

    #region ISBN

    public static ISBN ConvertFromDto(this AuthorBookDTO dto, List<Author> authList)
    {
        return new ISBN()
        {
            Isbn = dto.Isbn,
            Title = dto.Title,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate,
            Author = authList,
        };
    }
    public static ISBNDTORead ConvertToDto(this ISBN isbn)
    {
        return new ISBNDTORead
        {
            Isbn = isbn.Isbn,
            Title = isbn.Title,
            Description = isbn.Description,
            ReleaseDate = isbn.ReleaseDate,
            Authors = isbn.Author.ConvertToStrings(),
        };
    }
    public static ISBNDTORead ConvertToDto(this ISBN isbn, double avgRating)
    {
        var dto = isbn.ConvertToDto();
        dto.AvgRating = avgRating;
        return dto;
    }

    #endregion

    #region Helpers

    public static List<int> ConvertToIds(this List<Author> authorList) => authorList.Select(a => a.Id).ToList();

    public static List<string?> ConvertToStrings(this List<Author> authorList) =>
        authorList.Select(a => a.Person?.ToString()).ToList();


    #endregion
}

