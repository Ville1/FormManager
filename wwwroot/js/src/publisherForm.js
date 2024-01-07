import { createRoot } from 'react-dom/client';
import React from 'react';
import FormManager from '../components/formManager.js';

class PublisherForm extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <FormManager
                    url="/Publisher"
                    title={localization.Publisher}
                    rows={[
                        {
                            label: localization.Name,
                            property: 'name',
                            required: true
                        }
                    ]}
                />
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<PublisherForm />);