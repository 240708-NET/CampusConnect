import { createContext, ReactNode, useState } from 'react';
import axios from 'axios';

const defaultUser = {
    username : "Sample",
    isAdmin : false,
    posts : [{
        id: 1,
        topic: "sample post",
        body: "sample content",
        postCategory: "misc",
        poster: "Sample",
        createdAt: new Date(),
        editedAt: new Date(),
        tags: []
    }],
    comments: [{
        id: 1,
        body: 'general comment',
        createdAt: new Date(),
        editedAt: new Date()
    }]
}

// Define the User type
interface User {
    username: string,
    isAdmin: boolean,
    posts: Post[],
    comments: Comment[]
}

// Define the Post type
interface Post {
    id: number;
    topic: string;
    body: string;
    postCategory: string;
    poster: string;
    createdAt: Date;
    editedAt: Date;
    tags: string[];
}

// Define the Comment type
interface Comment {
    id: number;
    body: string;
    createdAt: Date;
    editedAt: Date;
}

interface UserContextProps {
    user: User,
    fetchUser: (username: string) => void;
}

export const UserContext = createContext<UserContextProps | undefined>(undefined);

interface UserProviderProps {
    children: ReactNode
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {

    const [user, setUser] = useState( defaultUser );

    const fetchUser = async (username: string) => { 

        try {
            const response = await axios.get(`/api/user/`);
            setUser(response.data.filter((user: User)=> user.username));
        } catch (error) {
            console.error('Error getting user', error);
        }
    }

    return(
        <UserContext.Provider value = {{ user: user, fetchUser }}>
            {children}
        </UserContext.Provider>
    )
}