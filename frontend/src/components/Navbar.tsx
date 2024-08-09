// src/components/Navbar/Navbar.tsx
import React, { useContext } from 'react';
import '../style/Navbar.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { UserContext } from '../context/UserContext';
import { Link } from 'react-router-dom';


const Navbar: React.FC = () => {
  const context = useContext(UserContext);

  const user = context?.user;

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <a className="navbar-brand" href="#">CampusConnect</a>
      <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span className="navbar-toggler-icon"></span>
      </button>
      <div className="collapse navbar-collapse" id="navbarNav">
        <ul className="navbar-nav ml-auto">
          <li className="nav-item">
            <Link className="nav-link" to="/">Home</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/profile">Profile</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to={ user ? "/logout" : "/login"}> { user ? `Logout, ${user.username}` : "Login"}</Link> 
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;
