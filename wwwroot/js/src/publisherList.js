import { createRoot } from 'react-dom/client';
import React from 'react';
import TableManager from '../components/tableManager.js';

class PublisherList extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="small main-content container">
                <h3 className="text-center">{localization.Publishers}</h3>
                <TableManager
                    url="/Publisher/Search"
                    typeName={localization.Publisher}
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
                            onClick: (publisher) => { window.location = baseUrl + '/Publisher/Form?id=' + publisher.id; }
                        },
                        {
                            icon: 'fa-solid fa-x',
                            tooltip: localization.Delete,
                            deleteFormUrl: '/Publisher'
                        }
                    ]}
                    buttons={[
                        {
                            text: localization.New,
                            href: baseUrl + '/Publisher/Form'
                        }
                    ]}
                />
            </div>
        );
    }
}

createRoot(document.getElementById('root')).render(<PublisherList />);