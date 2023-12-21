using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using LibraryApplicationProject.Data.DTO;

namespace LibraryApplicationProject.Data;

public class Author
{
    public int Id { get; set; }
    public string? Description { get; set; } = string.Empty;
    public Person? Person { get; set; }
    public List<ISBN> Isbn { get; set; } = new();

    public override string? ToString()
    {

        return $"{Person}\n Authors Id: {Id}, Description: {Description}, {Isbn.Aggregate(string.Empty, (s, i) => $"{i.Title}, {i.ReleaseDate.ToShortDateString()}, ISBN:{i.Isbn}")}";

    }


}

