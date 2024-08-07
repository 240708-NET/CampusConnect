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
    <div className="card mb-3">
      <div className="card-body">
        <h5 className="card-title">{post.title}</h5>
        <p className="card-text">{post.content}</p>
      </div>
    </div>
  );
};

export default PostCard;
