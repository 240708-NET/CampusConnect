// Import React and types
import React from 'react';

// Define types for PostCard props
interface PostCardProps {
  post: {
    id: number;
    title: string;
    content: string;
  };
}

// PostCard component
const PostCard: React.FC<PostCardProps> = ({ post }) => {
  return (
    <div className="post-card">
      <h3>{post.title}</h3> {/* Display post title */}
      <p>{post.content}</p> {/* Display post content */}
    </div>
  );
}

export default PostCard;
