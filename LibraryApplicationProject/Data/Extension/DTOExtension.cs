using LibraryApplicationProject.Data.DTO;
using Microsoft.CodeAnalysis.Operations;

namespace LibraryApplicationProject.Data.Extension;

public static class DTOExtension
{
    public static BookDTO? ConvertToDto(this Book book)
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
            Authors = book.Isbn.Author.ConvertToStrings(),
        };
    }

    public static (Person, Author) ConvertFromDto(this AuthorDTORead dtoRead)
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
        dto.RegistryDate = dto.RegistryDate <= DateTime.Today ? DateTime.Now : dto.RegistryDate;
        Membership membership = new Membership
        {
            CardNumber = dto.CardNumber,
            RegistryDate = dto.RegistryDate,
            ExpirationDate = dto.ExpirationDate,
            Person = person
        };
        return (person, membership);
    }



    #region Helpers

    public static List<int> ConvertToIds(this List<Author> authorList) => authorList.Select(a => a.Id).ToList();

    public static List<string?> ConvertToStrings(this List<Author> authorList) =>
        authorList.Select(a => a.Person?.ToString()).ToList();


    #endregion
}

