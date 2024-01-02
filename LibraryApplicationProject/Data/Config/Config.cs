using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApplicationProject.Data.Config
{
    public class PersonEntityTypeConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(t => t.Id).HasName("PrimaryKey_PersonId");
            builder.ToTable(t => t.HasComment("Register Person for Authors and Clients"));
            builder.Property(p => p.FirstName).HasColumnType("varchar(128)");
            builder.Property(p => p.LastName).HasColumnType("varchar(128)");
            builder.Property(p => p.BirthDate).HasColumnType("date").HasDefaultValue(DateOnly.FromDateTime(DateTime.Today));
        }
    }

    public class AuthorEntityTypeConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id).HasName("PrimaryKey_AuthorId");
            builder.Property(a => a.Description).HasColumnType("varchar(2000)");
        }
    }


    public class ISBNEntityTypeConfig : IEntityTypeConfiguration<ISBN>
    {
        public void Configure(EntityTypeBuilder<ISBN> builder)
        {
            builder.HasKey(i => i.Isbn_Id).HasName("PrimaryKey_ISBN");
            builder.Property(i => i.Isbn).HasColumnType("bigint");
            builder.HasIndex(i => i.Isbn).IsUnique();
            builder.Property(i => i.Isbn_Id).HasColumnType("bigint");
            builder.Property(i => i.Title).HasColumnType("varchar(128)");
            builder.Property(i => i.Description).HasColumnType("varchar(1200)");
            builder.Property(i => i.ReleaseDate).HasColumnType("date").HasDefaultValue(DateOnly.FromDateTime(DateTime.Today));
        }
    }

    public class MembershipEntityTypeConfig : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(m => m.Id).HasName("PrimaryKey_MembershipId");
            builder.Property(m => m.RegistryDate).HasColumnType("date").HasDefaultValue(DateOnly.FromDateTime(DateTime.Today));
            builder.Property(m => m.CardNumber).HasColumnType("bigint");
            builder.HasIndex(m => m.CardNumber).IsUnique();
        }
    }

    public class LoanEntityTypeConfig : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.Id).HasName("PrimaryKey_LoanId");
            builder.Property(l => l.StartDate).HasColumnType("date").HasDefaultValue(DateOnly.FromDateTime(DateTime.Today));
            builder.Property(l => l.EndDate).HasColumnType("date").HasDefaultValue(DateOnly.FromDateTime(DateTime.Today));
            builder.Property(l => l.IsActive).HasColumnType("bit").HasDefaultValue(0);
        }
    }

    public class BookEntityTypeConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id).HasName("PrimaryKey_BookId");
            builder.Property(b => b.IsAvailable).HasColumnType("bit");
        }
    }

    public class RatingEntityTypeConfig : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id).HasName("PrimaryKey_RatingId");
            builder.Property(r => r.ReaderRating).HasColumnType("tinyint").HasDefaultValue(0);
        }
    }



}
