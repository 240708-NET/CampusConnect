// src/context/PostContext.tsx

import React, { createContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';

// Define the Post type
interface Post {
  id: number;
  topic: string;
  body: string;
  categoryId: number;
  posterId: number;
  createdAt: Date;
  editedAt: Date;
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

interface PostTag {
  id: number;
  postId: number;
  tagId: number;
}

// Define the PostContextProps type
interface PostContextProps {
  posts: Post[];
  categories: Category[];
  tags: Tag[];
  postTags: PostTag[];
  fetchPosts: () => void;
  fetchCategories: () => void;
  fetchTags: () => void;
  fetchPostTags: () => void;
  filterByCategory: (category: string) => void;
  filterByTags: (tags: string[]) => void;
  addPost: (topic: string, body: string, category: string, posterId: number, tags: string[]) => void;
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
  const [postTags, setPostTags] = useState<PostTag[]>([]);
  const [filteredPosts, setFilteredPosts] = useState<Post[]>([]);

  // Function to fetch posts from the API
  const fetchPosts = async () => {
    try {
      const response = await axios.get('http://localhost:5226/api/post');
      setPosts(response.data);
      setFilteredPosts(response.data); // Initialize filteredPosts with all posts
    } catch (error) {
      console.error('Error fetching posts:', error);
    }
  };

  // Function to fetch categories from the API
  const fetchCategories = async () => {
    try {
      const response = await axios.get('http://localhost:5226/api/category');
      setCategories(response.data);
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  };

  // Function to fetch tags from the API
  const fetchTags = async () => {
    try {
      const response = await axios.get('http://localhost:5226/api/tag');
      setTags(response.data);
    } catch (error) {
      console.error('Error fetching tags:', error);
    }
  };

  const fetchPostTags = async () => {
    try {
      const response = await axios.get('http://localhost:5226/api/postTag');
      setPostTags(response.data);
    } catch (error) {
      console.error('Error fetching postTags:', error);
    }
  };

  // Function to filter posts by category
  const filterByCategory = (category: string) => {
    const finalCat = categories.filter(categoryItem => category == categoryItem.name )[0];
    console.log(finalCat);
    const filtered = posts.filter(post => post.categoryId === finalCat.id);
    console.log(filtered);
    setFilteredPosts(filtered);
  };

  // Function to filter posts by tags
  const filterByTags = (selectedTags: string[]) => {
    const searchTag = tags.filter(tag => selectedTags.includes(tag.name));
    const matchingPost = postTags.filter(postTag => searchTag.find(tag => tag.id==postTag.tagId));
    const filtered = posts.filter(post => matchingPost.find(postTag => postTag.postId==post.id));
    setFilteredPosts(filtered);
  };

  const addPost = async (iTopic: string, iBody: string, iCategory: string, iPosterId: number, iTags: string[]) => {
    const categoryID = categories.find(searchCategory => searchCategory.name == iCategory)?.id;
    const tagList = iTags.map(iTag => tags.find(tag=> tag.name == iTag) ? tags.find(tag=> tag.name == iTag)?.id : iTag );
    try{
      const response = await axios.post('http://localhost:5226/api/post',{
        topic: iTopic, body: iBody, posterId: iPosterId, categoryId: categoryID
      });
      const postId = response.data.id;
      for(let i = 0; i < tagList.length; i++) {
        if(typeof tagList[i] == "string") {
          console.log(tagList[i]);
          const newTag = await axios.post('http://localhost:5226/api/tag',{ tag: tagList[i] });
          tagList[i] = newTag.data.id;
        }
        const newPostTag = await axios.post('http://localhost:5226/api/postTag', {
          tag: tagList[i], postId: postId
        });
      }
      fetchPosts();
    } catch (error) {
      console.log(error);
    }
  }

  // Fetch data when the component mounts
  useEffect(() => {
    fetchPosts();
    fetchCategories();
    fetchTags();
    fetchPostTags();
  }, []);

  // Provide context value to children
  return (
    <PostContext.Provider value={{ posts: filteredPosts, categories, tags, postTags, fetchPosts, fetchCategories, fetchTags, fetchPostTags, filterByCategory, filterByTags, addPost }}>
      {children}
    </PostContext.Provider>
  );
};
