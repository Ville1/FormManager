import { createRoot } from 'react-dom/client';
import React from 'react';
import FormManager from '../components/formManager.js';

class VideoGameForm extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <FormManager
                    url="/VideoGame"
                    title={localization.VideoGame}
                    rows={[
                        {
                            label: localization.Name,
                            property: 'name',
                            required: true
                        },
                        {
                            inputs: [
                                {
                                    label: localization.Developer,
                                    type: 'dropdown',
                                    property: 'developerId',
                                    options: pDevelopers,
                                    required: true
                                },
                                {
                                    label: localization.Publisher,
                                    type: 'dropdown',
                                    property: 'publisherId',
                                    options: pPublishers,
                                    required: true
                                }
                            ]
                        }
                    ]}
                />
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<VideoGameForm />);