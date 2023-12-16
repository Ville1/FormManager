import { createRoot } from 'react-dom/client';
import React from 'react';

class FrontPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <h3 className="text-center">{'Moro'}</h3>
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<FrontPage />);