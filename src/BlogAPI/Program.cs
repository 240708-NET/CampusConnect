using Microsoft.EntityFrameworkCore;
using BlogApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("BlogPlatform"));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
