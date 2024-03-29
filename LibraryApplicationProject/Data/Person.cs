﻿using LibraryApplicationProject.Data.DTO;

namespace LibraryApplicationProject.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public override string ToString() => $"{FirstName} {LastName}";
    }
}