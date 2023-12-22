﻿// <auto-generated />
using System;
using LibraryApplicationProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20231221151104_Loan")]
    partial class Loan
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("library")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthorISBN", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<long>("Isbn_Id")
                        .HasColumnType("bigint");

                    b.HasKey("AuthorId", "Isbn_Id");

                    b.HasIndex("Isbn_Id");

                    b.ToTable("AuthorISBN", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("varchar(2000)");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_AuthorId");

                    b.HasIndex("PersonId");

                    b.ToTable("Authors", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<long?>("Isbn_Id")
                        .HasColumnType("bigint");

                    b.Property<int?>("LoanId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_BookId");

                    b.HasIndex("Isbn_Id");

                    b.HasIndex("LoanId");

                    b.ToTable("Books", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.ISBN", b =>
                {
                    b.Property<long>("Isbn_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Isbn_Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1200)");

                    b.Property<long>("Isbn")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ReleaseDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.HasKey("Isbn_Id")
                        .HasName("PrimaryKey_ISBN");

                    b.HasIndex("Isbn")
                        .IsUnique();

                    b.ToTable("ISBNs", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

                    b.Property<int?>("MembershipId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

                    b.HasKey("Id")
                        .HasName("PrimaryKey_LoanId");

                    b.HasIndex("MembershipId");

                    b.ToTable("Loans", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Membership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("CardNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistryDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

                    b.HasKey("Id")
                        .HasName("PrimaryKey_MembershipId");

                    b.HasIndex("CardNumber")
                        .IsUnique();

                    b.HasIndex("PersonId");

                    b.ToTable("Memberships", "library");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_PersonId");

                    b.ToTable("Persons", "library", t =>
                        {
                            t.HasComment("Register Person for Authors and Clients");
                        });
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long?>("Isbn_Id")
                        .HasColumnType("bigint");

                    b.Property<int?>("MembershipId")
                        .HasColumnType("int");

                    b.Property<byte>("ReaderRating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)2);

                    b.HasKey("Id")
                        .HasName("PrimaryKey_RatingId");

                    b.HasIndex("Isbn_Id");

                    b.HasIndex("MembershipId");

                    b.ToTable("Rating", "library");
                });

            modelBuilder.Entity("AuthorISBN", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryApplicationProject.Data.ISBN", null)
                        .WithMany()
                        .HasForeignKey("Isbn_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Author", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Book", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.ISBN", "Isbn")
                        .WithMany()
                        .HasForeignKey("Isbn_Id");

                    b.HasOne("LibraryApplicationProject.Data.Loan", null)
                        .WithMany("Books")
                        .HasForeignKey("LoanId");

                    b.Navigation("Isbn");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Loan", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.Membership", "Membership")
                        .WithMany()
                        .HasForeignKey("MembershipId");

                    b.Navigation("Membership");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Membership", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Rating", b =>
                {
                    b.HasOne("LibraryApplicationProject.Data.ISBN", "Isbn")
                        .WithMany()
                        .HasForeignKey("Isbn_Id");

                    b.HasOne("LibraryApplicationProject.Data.Membership", "Membership")
                        .WithMany()
                        .HasForeignKey("MembershipId");

                    b.Navigation("Isbn");

                    b.Navigation("Membership");
                });

            modelBuilder.Entity("LibraryApplicationProject.Data.Loan", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
