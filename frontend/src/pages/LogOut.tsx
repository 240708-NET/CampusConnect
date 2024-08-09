import React, { useContext, useState } from 'react';
import { UserContext } from '../context/UserContext';
import { Navigate }  from 'react-router-dom';

const LogOut: React.FC = () => {

    const context = useContext(UserContext);

    if (!context) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    

    const { logOut } = context;

    logOut();

    return <Navigate to="/" />;

    
}

export default LogOut;