using System.Diagnostics;
using System.Text.Json.Serialization;
using LibraryApplicationProject;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var t = builder.Configuration.GetConnectionString("SQLDataString");
Console.WriteLine($"Connection String: {t} \n");
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<LibraryDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLDataString")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
{
     "id": 0,
     "isbn": 9700765365293,
     "title": "Oathbringer",
     "description": "Ash and red lightning sweep the lands...",
     "releaseDate": "2023-12-20T15:14:18.280Z",
     "authorId": [
       2
     ],
     "quantity": 5,
     "isAvailable": true
   }

{
     "id": 0,
     "title": "The Gamification of learning and instruction",
     "description": "Praise for The Gamification of Learning and Instruction",
     "isbn": 9781118096345,
     "releaseDate": "2023-12-20T15:50:25.281Z",
     "quantity": 3,
     "authors": [
       {
         "id": 0,
         "firstName": "Karl",
         "lastName": "M. Kapp",
         "birthDate": "2023-12-20T15:50:25.281Z",
         "description": "Writer of The Gamification of Learning and Instruction"
       }
     ]
   }
*/