import { createRoot } from 'react-dom/client';
import React from 'react';
import TableManager from '../components/tableManager.js';

class DeveloperList extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <h3 className="text-center">{localization.Developers}</h3>
                <TableManager
                    url="/Developer/Search"
                    columns={[
                        {
                            title: 'Name',
                            propertyName: 'name'
                        }
                    ]}
                    filters={[
                        {
                            label: 'Name',
                            propertyName: 'name'
                        }
                    ]}
                    rowButtons={[
                        {
                            icon: 'fa-solid fa-pen',
                            tooltip: localization.Edit,
                            onClick: (developer) => { window.location = baseUrl + '/Developer/Form?id=' + developer.id; }
                        },
                        {
                            icon: 'fa-solid fa-x',
                            tooltip: localization.Delete,
                            deleteFormUrl: '/Developer'
                        }
                    ]}
                    buttons={[
                        {
                            text: localization.New,
                            href: baseUrl + '/Developer/Form'
                        }
                    ]}
                />
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<DeveloperList />);