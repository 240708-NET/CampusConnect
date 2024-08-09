import { createContext, ReactNode, useState } from 'react';
import axios from 'axios';

// Define the User type
interface User {
    id: number,
    username: string,
    password: string,
    isAdmin: boolean,
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
    user: User | undefined,
    fetchUser: (username: string, password: string) => void;
    addUser: (username: string, password: string) => void;
    logOut: () => void;
}

export const UserContext = createContext<UserContextProps | undefined>(undefined);

interface UserProviderProps {
    children: ReactNode
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {

    const [user, setUser] = useState<User>( );

    const fetchUser = async (username: string, password: string) => {
        try {
            const response = await axios.get(`http://localhost:5226/api/user`);
            setUser(response.data.filter((user: User)=> user.username == username)[0]);
        } catch (error) {
            console.error('Error getting user', error);
        }
    }

    const addUser = async (username: string, password: string) => {
        let tempUser: User | undefined;

        try {
            const response = await axios.get(`http://localhost:5226/api/user`);
            tempUser = (response.data.filter((user: User)=> user.username == username)[0]);

            if(!tempUser) {
                const response = await axios.post(`http://localhost:5226/api/user`, {
                    username: username, password: password
                });
                setUser(response.data);
            } 
        } catch (error) {
            console.error(error);
        }
    }

    const logOut = () => {
        setUser(undefined);
    }

    return(
        <UserContext.Provider value = {{ user: user, fetchUser, addUser, logOut }}>
            {children}
        </UserContext.Provider>
    )
}