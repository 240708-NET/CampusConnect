// src/components/Navbar/Navbar.tsx
import React from 'react';
import './Navbar.css'; // Import the CSS file for styling

const Navbar: React.FC = () => {
  return (
    <nav className="navbar">
      <div className="navbar-brand">CampusConnect</div>
      <ul className="navbar-nav">
        <li><a href="#">Home</a></li>
        <li><a href="#">Posts</a></li>
        <li><a href="#">Profile</a></li>
        <li><a href="#">Logout</a></li>
      </ul>
    </nav>
  );
};

export default Navbar;
