// src/components/MainPage/MainPage.tsx
import React, { useContext, useEffect, useState } from 'react';
import { PostContext, PostProvider } from '../context/PostContext';
import PostList from '../components/PostList';
import Navbar from '../components/Navbar';
import { Link } from 'react-router-dom';

const MainPage: React.FC = () => {
  const context = useContext(PostContext);
  const [selectedCategory, setSelectedCategory] = useState<string>('');
  const [selectedTags, setSelectedTags] = useState<string[]>([]);

  if (!context) {
    return <div>Loading...</div>;
  }

  const { posts, categories, tags, fetchCategories, fetchPosts, filterByCategory, filterByTags } = context;

  const handleCategoryChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const category = event.target.value;
    setSelectedCategory(category);
    filterByCategory(category);
  };

  const handleTagChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedOptions = Array.from(event.target.selectedOptions, option => option.value);
    setSelectedTags(selectedOptions);
    filterByTags(selectedOptions);
  };

  useEffect(() => {
    fetchPosts();
    console.log("Use effect in main");
  }, [posts.length]);

  return (
      <div>
      <Navbar />
      <div className="main-page container">
        <h2>CampusConnect Blog</h2>
        <Link className="btn btn-primary mb-3" style={{ width: '100%' }} to="/post">Create Post</Link>
        <div className="row mb-3">
          <div className="col-md-6">
            <label htmlFor="categorySelect">Filter by Category</label>
            <select id="categorySelect" className="form-control" value={selectedCategory} onChange={handleCategoryChange}>
              <option value="">All Categories</option>
              {categories.map((category: { id: number, name: string }) => (
                <option key={category.id} value={category.name}>{category.name}</option>
              ))}
            </select>
          </div>
          <div className="col-md-6">
            <label htmlFor="tagsSelect">Filter by Tags</label>
            <select id="tagsSelect" className="form-control" multiple value={selectedTags} onChange={handleTagChange}>
              {tags.map((tag: { id: number, name: string }) => (
                <option key={tag.id} value={tag.id}>{tag.name}</option>
              ))}
            </select>
          </div>
        </div>
        <PostList />
      </div>
      </div>

  );
}

export default MainPage;
