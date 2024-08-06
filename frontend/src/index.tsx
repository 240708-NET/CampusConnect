// Import React and ReactDOM for rendering the application
import React from 'react';
import ReactDOM from 'react-dom';
import './styles.css'; // Import global styles
import App from './App'; // Import the main App component

// Render the App component inside the root div in public/index.html
ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById('root') // This references the root div in the HTML file
);
