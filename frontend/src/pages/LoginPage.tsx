import React, { useContext, useState } from 'react';
// import UserPosts from './UserPosts';
import { UserContext } from '../context/UserContext';
import Navbar from '../components/Navbar';
import { Navigate }  from 'react-router-dom';

const LoginPage: React.FC = () => {

    const context = useContext(UserContext);
    const [ userField, setUserField ] = useState('');
    const [ passField, setPassField ] = useState('');

    if (!context) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    const { user, fetchUser, addUser } = context;

    if(user) {
        console.log(user);
        return <Navigate to="/" />;
    }

    const userChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setUserField(e.target.value)
    }

    const passChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPassField(e.target.value)
    }

    const logOn = (e: React.ChangeEvent<HTMLFormElement>) => {
        e.preventDefault()
        fetchUser(userField, passField);
        return <Navigate to="/" />;
    }

    const signUp = () => {
        addUser(userField, passField);
        return <Navigate to="/" />; 
    }
    
    return (
        <div>
            <Navbar />
            <div className="profile-page container">
                <form onSubmit={logOn}>
                    <label htmlFor="username">Username</label>
                    <input type="text" id="username" name="username" value={userField} onChange={userChange} required></input>
                    <label htmlFor="password">Password</label>
                    <input type="password" id="password" name="password" value={passField} onChange={passChange} required></input>
                    <button type="submit" value="login">Login</button>
                    <button type="submit" value="signup" onClick={signUp}>Sign Up</button>
                </form>
            </div>
        </div>
    )
}

export default LoginPage;