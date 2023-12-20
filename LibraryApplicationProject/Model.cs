namespace LibraryApplicationProject;
using Data;
using Data.Config;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


public class LibraryDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<ISBN> ISBNs { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Rating> Rating { get; set; }


    private readonly string _connectionString = "";
    public LibraryDbContext()
    {
    }
    public LibraryDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.LogTo(message => Debug.WriteLine(message)).EnableSensitiveDataLogging();
        if (!options.IsConfigured)
        {
            var connectionString = _connectionString;
            if (connectionString == string.Empty)
            {
                connectionString = ConnectionString.SqlString;
                //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }
            options.UseSqlServer(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("library");
        new PersonEntityTypeConfig().Configure(modelBuilder.Entity<Person>());
        new AuthorEntityTypeConfig().Configure(modelBuilder.Entity<Author>());
        new ISBNEntityTypeConfig().Configure(modelBuilder.Entity<ISBN>());
        new MembershipEntityTypeConfig().Configure(modelBuilder.Entity<Membership>());
        new LoanEntityTypeConfig().Configure(modelBuilder.Entity<Loan>());
        new BookEntityTypeConfig().Configure(modelBuilder.Entity<Book>());
        new RatingEntityTypeConfig().Configure(modelBuilder.Entity<Rating>());

    }

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>()
            .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
    }*/
}

