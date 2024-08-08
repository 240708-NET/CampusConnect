import React, { useContext, useState } from 'react';
import UserPosts from './UserPosts';
import { UserContext } from '../../context/UserContext';

const ProfilePage: React.FC = () => {

    const context = useContext(UserContext);

    if (!context) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    const { user, fetchUser } = context;

    // fetchUser("testUser");

    return (
        <div className="profile-page container">
            <h2>{user.username}'s Profile{user.isAdmin && <span> (Admin)</span>}</h2>
            <div>
                <UserPosts />
            </div>
        </div>
    )
}

export default ProfilePage;