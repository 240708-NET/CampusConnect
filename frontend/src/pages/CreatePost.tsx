import React, { useContext, useState } from 'react';
import { UserContext } from '../context/UserContext';
import Navbar from '../components/Navbar';
import { Navigate }  from 'react-router-dom';
import { PostContext } from '../context/PostContext';

interface PostProps {
    props: {
        topic: string;
        body: string;
        category: string;
        tags: string[];
    } | undefined;
}

const CreatePost: React.FC<PostProps> = ({props}) => {

    const userContext = useContext(UserContext);
    const postContext = useContext(PostContext);

    const [topic, setTopic] = useState('');
    const [body, setBody] = useState('');
    const [category, setCategory] = useState('');
    const [tags, setTags] = useState<string[]>([]);

    if (!postContext) {
        return <div>Loading...</div>; // Display loading message if context is not available
    }

    const { categories, fetchCategories, addPost } = postContext;

    if(!userContext || !userContext.user) {
        return <Navigate to="/" />;
    }

    const { user } = userContext;

    if(props) {
        setTopic(props.topic);
        setBody(props.body);
        setCategory(props.category);
        setTags(props.tags);
    }

    const makePost = (e: React.ChangeEvent<HTMLFormElement>) => {
        e.preventDefault();
        if(tags[0] == '') setTags([]);
        console.log(tags);
        addPost(topic, body, category, user.id, tags);
    }

    const handleTopicChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setTopic(e.target.value);
    }

    const handleBodyChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setBody(e.target.value);
    }

    const handleCategoryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setCategory(e.target.value);
    }

    const handleTagChange = (e: React.ChangeEvent<HTMLInputElement>) => {{
        setTags(e.target.value.split(", ").join(" ").split(" ").join(",").split(","));
    }}

    
    
    return (
        <div>
            <Navbar />
            <div className="profile-page container">
                <form onSubmit={makePost}>
                    <label htmlFor='topic'>Topic:</label>
                    <input type="text" id="topic" name="topic" value={topic} onChange={handleTopicChange}/>
                    <label htmlFor='body'>Body:</label>
                    <textarea id="body" name="body" value={body} onChange={handleBodyChange}/>
                    <label htmlFor='category'>Category:</label>
                    <select name="category" value={category} onChange={handleCategoryChange}>
                        {categories.map((category: { id: number, name: string }) => (
                            <option key={category.id} value={category.name}>{category.name}</option>
                        ))}
                    </select>
                    <label htmlFor='tags'>Tags:</label>
                    <input type='text' id="tags" name="tags" value={tags.toString()} onChange={handleTagChange}/>
                    <button type="submit">{ props && "Update"} Post</button>
                </form>
            </div>
        </div>
    )
}

export default CreatePost;