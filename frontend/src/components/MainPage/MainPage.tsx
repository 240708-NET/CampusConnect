// Import React and other necessary components
import React from 'react';
import PostList from './PostList'; // Import PostList component

const MainPage: React.FC = () => {
  return (
    <div className="container mt-5">
      <h2 className="mb-4">CampusConnect Blog</h2>
      <PostList />
    </div>
  );
}

export default MainPage;
