using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinApi.Data;
using SixMinApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder
{
    ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection"),
    UserID = builder.Configuration["UserId"],
    Password = builder.Configuration["Password"]
};

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/commands", async (ICommandRepo repo, IMapper mapper) => {
   var commands = await repo.GetAllCommands();
   return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands)); 
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) => {
    var command = await repo.GetCommandById(id);
    if (command != null)
    {
        return Results.Ok(mapper.Map<CommandReadDto>(command));
    }
    return Results.NotFound();
    });

app.Run();
