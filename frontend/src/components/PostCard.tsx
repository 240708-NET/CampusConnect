// Import React and types
import React from 'react';

// Define types for PostCard props
interface PostCardProps {
  post: {
    id: number;
    topic: string;
    body: string;
    categoryId: number;
    posterId: number;
    createdAt: Date;
    editedAt: Date;
  }
}

// PostCard component
const PostCard: React.FC<PostCardProps> = ({ post }) => {
  return (
    <div className="card mb-3">
      <div className="card-body">
        <h5 className="card-title">{post.topic}</h5>
        <p className="card-text">{post.body}</p>
      </div>
    </div>
  );
};

export default PostCard;
