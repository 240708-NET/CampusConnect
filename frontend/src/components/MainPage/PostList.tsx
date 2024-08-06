import React, { useContext, useEffect } from 'react';
import { PostContext } from '../../context/PostContext';
import PostCard from './PostCard';

const PostList: React.FC = () => {
  const context = useContext(PostContext);

  // Ensure context is defined before proceeding
  if (!context) {
    return <div>Loading...</div>; // Display loading message if context is not available
  }

  const { posts, fetchPosts } = context;

  // Fetch posts when the component mounts
  useEffect(() => {
    fetchPosts(); // Fetch posts as soon as the component is rendered
  }, [fetchPosts]); // Dependency array ensures effect runs when fetchPosts changes

  return (
    <div className="post-list">
      {/* Map through posts and render a PostCard for each */}
      {posts.map((post) => (
        <PostCard key={post.id} post={post} />
      ))}
    </div>
  );
};

export default PostList;
