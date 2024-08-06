// Import React and other necessary components
import React from 'react';
import PostList from './PostList'; // Import PostList component

// MainPage component
const MainPage: React.FC = () => {
  return (
    <div className="main-page">
      <h2>CampusConnect Blog</h2> {/* Title of the main page */}
      <PostList /> {/* Render the PostList component */}
    </div>
  );
}

export default MainPage;
