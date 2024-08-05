using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("BlogPlatform"));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(sp.GetRequiredService<BlogContext>()));
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
