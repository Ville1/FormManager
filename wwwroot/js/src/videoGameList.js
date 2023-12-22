import { createRoot } from 'react-dom/client';
import React from 'react';
import TableManager from '../components/tableManager.js';

class VideoGameList extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <TableManager
                />
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<VideoGameList />);