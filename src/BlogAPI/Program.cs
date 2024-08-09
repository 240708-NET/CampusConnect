using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
        options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader(); 
        });
});

// Configure Entity Framework
builder.Services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("BlogPlatform"));

// Configure Dependecy Injection for Repositories
builder.Services.AddScoped<ICategoryRepository>(sp => new CategoryRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<ICommentRepository>(sp => new CommentRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IPostRepository>(sp => new PostRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IPostTagRepository>(sp => new PostTagRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<ITagRepository>(sp => new TagRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(sp.GetRequiredService<BlogContext>()));

// Configure MVC
builder.Services.AddControllers(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
