using LibraryApplicationProject.Data.DTO;
using System;
namespace LibraryApplicationProject.Data.Extension;

public static class DTOExtension
{
    public static BookDTO ConvertToDto(this Book book)
    {
        if (book.Isbn == null)
            return null;
        return new BookDTO
        {
            Id = book.Id,
            IsAvailable = book.IsAvailable,
            Isbn = book.Isbn.Isbn,
            Title = book.Isbn.Title,
            Description = book.Isbn.Description,
            ReleaseDate = book.Isbn.ReleaseDate,
            Authors = book.Isbn.Author.ConvertToIds(),
        };
    }
    public static BookSearchDTO ConvertToDtoRead(this Book book)
    {
        if (book.Isbn == null)
            return null;
        return new BookSearchDTO
        {
            Id = book.Id,
            Isbn = book.Isbn.Isbn,
            Title = book.Isbn.Title,
            Description = book.Isbn.Description,
            ReleaseDate = book.Isbn.ReleaseDate,
            Authors = book.Isbn.Author.ConvertToStrings(),
        };
    }

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
        var authorDto = new AuthorDTORead
        {
            Id = author.Id,
            FirstName = pNull ? "" : author.Person.FirstName,
            LastName = pNull ? "" : author.Person.LastName,
            BirthDate = pNull ? DateOnly.FromDateTime(DateTime.Today) : author.Person.BirthDate,
            Description = author.Description,
            BooksList = author.Isbn.Select(i => i.ToString()).ToList(),
        };
        return authorDto;
    }

    public static Book ConvertFromDto(this BookEntryDTO dto, ISBN isbn)
    {
        var book = new Book
        {
            Isbn = isbn,
            IsAvailable = dto.IsAvailable
        };
        return book;
    }
    public static (Person, Membership) ConvertFromDto(this MembershipDTO dto)
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

    public static LoanDTORead ConvertToDto(this Loan loan)
    {
        var books = new List<BookDTO>();
        loan.Books.ForEach(x => books.Add(x.ConvertToDto()));

        var dto = new LoanDTORead
        {
            Id = loan.Id,
            MembershipCardNumber = loan.Membership.CardNumber,
            FirstName = loan.Membership.Person.FirstName,
            LastName = loan.Membership.Person.LastName,
            StartDate = loan.StartDate,
            ReturnDate = loan.EndDate,
            Books = books,
        };
        return dto;
    }

    public static Loan ConvertFromDto(this LoanDTOEntry loanDtoEntry, Membership member, List<Book> books)
    {

        var loan = new Loan
        {
            Membership = member,
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = loanDtoEntry.ReturnDate,
            IsActive = true,
            Books = books,
        };
        return loan;
    }

    public static ISBNDTOEntry ConvertToDto(this ISBN isbn)
    {
        var dto = new ISBNDTOEntry
        {
            Id = isbn.Isbn_Id,
            Isbn = isbn.Isbn,
            Title = isbn.Title,
            Description = isbn.Description,
            ReleaseDate = isbn.ReleaseDate,
            Authors = isbn.Author.ConvertToIds(),
        };

        return dto;
    }
    public static ISBNDTOEntry ConvertToDto(this ISBN isbn, double avgRating)
    {
        var dto = isbn.ConvertToDto();
        dto.AvgRating = avgRating;
        return dto;
    }


    #region Helpers

    public static List<int> ConvertToIds(this List<Author> authorList) => authorList.Select(a => a.Id).ToList();

    public static List<string?> ConvertToStrings(this List<Author> authorList) =>
        authorList.Select(a => a.Person?.ToString()).ToList();


    #endregion
}

