// Import React and other necessary components
import React from 'react';
import Navbar from './components/Navbar/Navbar'; // Import Navbar component
import MainPage from './components/MainPage/MainPage'; // Import MainPage component
import { PostProvider } from './context/PostContext'; // Import context provider
import './styles.css'; // Import global styles
import 'bootstrap/dist/css/bootstrap.min.css';


// Main App component
const App: React.FC = () => {
  return (
    // Wrap the application in PostProvider to provide context to all components
    <PostProvider>
      <div className="App">
        <Navbar /> {/* Render the Navbar component */}
        <MainPage /> {/* Render the MainPage component */}
      </div>
    </PostProvider>
  );
}

export default App;
