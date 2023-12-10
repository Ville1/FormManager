import { createRoot } from 'react-dom/client';
import React from 'react';

class FrontPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <h1>Moro</h1>;
    }
}

createRoot(document.getElementById('root')).render(<FrontPage />);