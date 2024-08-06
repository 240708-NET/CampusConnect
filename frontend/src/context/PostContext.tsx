import React, { createContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';

// Define the Post type
interface Post {
  id: number;
  title: string;
  content: string;
}

// Define the PostContextProps type
interface PostContextProps {
  posts: Post[];
  fetchPosts: () => void;
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

  // Function to fetch posts from the API
  const fetchPosts = async () => {
    try {
      const response = await axios.get('/api/posts');
      setPosts(response.data);
    } catch (error) {
      console.error('Error fetching posts:', error);
    }
  };

  // Fetch posts when the component mounts
  useEffect(() => {
    fetchPosts();
  }, []);

  // Provide context value to children
  return (
    <PostContext.Provider value={{ posts, fetchPosts }}>
      {children}
    </PostContext.Provider>
  );
};
