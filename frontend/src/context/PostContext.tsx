// src/context/PostContext.tsx

import React, { createContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';

// Define the Post type
interface Post {
  id: number;
  title: string;
  content: string;
  category: string;
  tags: string[];
}

// Define the Category type
interface Category {
  id: number;
  name: string;
}

// Define the Tag type
interface Tag {
  id: number;
  name: string;
}

// Define the PostContextProps type
interface PostContextProps {
  posts: Post[];
  categories: Category[];
  tags: Tag[];
  fetchPosts: () => void;
  fetchCategories: () => void;
  fetchTags: () => void;
  filterByCategory: (category: string) => void;
  filterByTags: (tags: string[]) => void;
}

// Create the context with a default value of undefined
export const PostContext = createContext<PostContextProps | undefined>(undefined);

// Define the type for the PostProvider props, including children
interface PostProviderProps {
  children: ReactNode; // Add this to specify the type of children
}

// Define the PostProvider component
export const PostProvider: React.FC<PostProviderProps> = ({ children }) => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [tags, setTags] = useState<Tag[]>([]);
  const [filteredPosts, setFilteredPosts] = useState<Post[]>([]);

  // Function to fetch posts from the API
  const fetchPosts = async () => {
    try {
      const response = await axios.get('/api/posts');
      setPosts(response.data);
      setFilteredPosts(response.data); // Initialize filteredPosts with all posts
    } catch (error) {
      console.error('Error fetching posts:', error);
    }
  };

  // Function to fetch categories from the API
  const fetchCategories = async () => {
    try {
      const response = await axios.get('/api/categories');
      setCategories(response.data);
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  };

  // Function to fetch tags from the API
  const fetchTags = async () => {
    try {
      const response = await axios.get('/api/tags');
      setTags(response.data);
    } catch (error) {
      console.error('Error fetching tags:', error);
    }
  };

  // Function to filter posts by category
  const filterByCategory = (category: string) => {
    const filtered = posts.filter(post => post.category === category);
    setFilteredPosts(filtered);
  };

  // Function to filter posts by tags
  const filterByTags = (selectedTags: string[]) => {
    const filtered = posts.filter(post =>
      selectedTags.every(tag => post.tags.includes(tag))
    );
    setFilteredPosts(filtered);
  };

  // Fetch data when the component mounts
  useEffect(() => {
    fetchPosts();
    fetchCategories();
    fetchTags();
  }, []);

  // Provide context value to children
  return (
    <PostContext.Provider value={{ posts: filteredPosts, categories, tags, fetchPosts, fetchCategories, fetchTags, filterByCategory, filterByTags }}>
      {children}
    </PostContext.Provider>
  );
};
