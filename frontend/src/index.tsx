// Import React and ReactDOM for rendering the application
import React from 'react';
import ReactDOM from 'react-dom';
import './styles.css'; // Import global styles
import App from './App'; // Import the main App component
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import MainPage from './pages/MainPage';
import ProfilePage from './pages/ProfilePage';
import LoginPage from './pages/LoginPage';
import LogOut from './pages/LogOut';
import CreatePost from './pages/CreatePost';
import { PostProvider } from './context/PostContext';
import { UserProvider } from './context/UserContext';

const router = createBrowserRouter([
  {
    path: '/',
    element: <MainPage />
  },
  {
    path: 'profile',
    element: <ProfilePage />
  },
  {
    path: 'login',
    element: <LoginPage />
  },
  {
    path: 'logout',
    element: <LogOut />
  },
  {
    path: 'post',
    element: <CreatePost props={ undefined }/>
  }

])

// Render the App component inside the root div in public/index.html
ReactDOM.render(
  <React.StrictMode>
    <UserProvider>
      <PostProvider>
        <RouterProvider router={router} />
      </PostProvider>
    </UserProvider>
  </React.StrictMode>,
  document.getElementById('root') // This references the root div in the HTML file
);
