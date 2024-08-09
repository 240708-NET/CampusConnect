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
                <form onSubmit={logOn} className="card p-4 mx-auto" style={{ maxWidth: '400px' }}>
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input type="text" id="username" name="username" className="form-control" value={userField} onChange={userChange} required />
                    </div>
                    <div className="form-group mt-3">
                        <label htmlFor="password">Password</label>
                        <input type="password" id="password" name="password" className="form-control" value={passField} onChange={passChange} required />
                    </div>
                    <div className="d-flex justify-content-between mt-4">
                        <button type="submit" className="btn btn-primary" value="login">Login</button>
                        <button type="button" className="btn btn-secondary" value="signup" onClick={signUp}>Sign Up</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default LoginPage;