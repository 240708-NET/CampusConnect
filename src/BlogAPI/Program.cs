using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("BlogPlatform"));
builder.Services.AddScoped<ICategoryRepository>(sp => new CategoryRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<ICommentRepository>(sp => new CommentRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IPostRepository>(sp => new PostRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IPostTagRepository>(sp => new PostTagRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<ITagRepository>(sp => new TagRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(sp.GetRequiredService<BlogContext>()));
builder.Services.AddControllers(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
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
