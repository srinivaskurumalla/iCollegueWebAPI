using iCollegueWebAPI.DTO;
using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using iCollegueWebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuration connection string information
builder.Services.AddDbContext<iColleagueContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("iColleagueConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Add services to the container.
builder.Services.AddScoped<IKnowledgeBase<TblKnowledgeBase>, KnowledgeBaseRepo>();
builder.Services.AddScoped<IKnowledgeBaseWithFilesDto<KnowledgeBaseDto>, KnowledgeBaseRepo>();


/*builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });
*/
builder.Services.AddControllers();
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
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
