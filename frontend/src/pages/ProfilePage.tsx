import React, { useContext, useState } from 'react';
// import UserPosts from './UserPosts';
import { UserContext } from '../context/UserContext';
import Navbar from '../components/Navbar';
import { Navigate }  from 'react-router-dom';

const ProfilePage: React.FC = () => {

    const context = useContext(UserContext);

    if (!context) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    

    const { user, fetchUser } = context;

    if ( !user ) {
        return <Navigate to="/login" />;
    }

    // fetchUser("testUser");

    return (
        <div>
            <Navbar />
            <div className="profile-page container">
                <h2>{user.username}'s Profile{user.isAdmin && <span> (Admin)</span>}</h2>
                <div>
                    {/* <UserPosts /> */}
                </div>
            </div>
        </div>
    )
}

export default ProfilePage;