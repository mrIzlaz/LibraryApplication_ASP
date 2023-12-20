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
        if(Person != null)
            return $"Name: {Person.FirstName} {Person.LastName}, Born: {Person.BirthDate}\n " +
               $"Author Id: {Id}, Description: {Description}";
        else
            return base.ToString();
    }
    

}

