import React, { useContext, useEffect } from 'react';
import { UserContext } from '../../context/UserContext';
import PostCard from './PostCard';

const UserPosts: React.FC = () => {

    const context = useContext(UserContext);

    if(!context) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    const { user, fetchUser } = context;

    return (
        <div>
            <h3>{user.username}'s Posts</h3>
            {user.posts.map (post => (
                <PostCard post = {post} />
            ))}
        </div>
    )
}

export default UserPosts;
