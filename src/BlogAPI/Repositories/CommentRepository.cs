namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class CommentRepository(BlogContext context) : GenericRepository<Comment>(context), ICommentRepository { }
