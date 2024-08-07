// src/components/MainPage/MainPage.tsx
import React, { useContext, useState } from 'react';
import { PostContext } from '../../context/PostContext';
import PostList from './PostList';

const MainPage: React.FC = () => {
  const context = useContext(PostContext);
  const [selectedCategory, setSelectedCategory] = useState<string>('');
  const [selectedTags, setSelectedTags] = useState<string[]>([]);

  if (!context) {
    return <div>Loading...</div>;
  }

  const { categories, tags, filterByCategory, filterByTags } = context;

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

  return (
    <div className="main-page container">
      <h2>CampusConnect Blog</h2>
      <button className="btn btn-primary mb-3" style={{ width: '100%' }}>Create Post</button>
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
              <option key={tag.id} value={tag.name}>{tag.name}</option>
            ))}
          </select>
        </div>
      </div>
      <PostList />
    </div>
  );
}

export default MainPage;
