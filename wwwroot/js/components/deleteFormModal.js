import React from 'react';
import { hasStringValue } from './utils.js';

/**
 * Modal used to confirm deletion of form and checking if form can be deleted
 */
class DeleteFormModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = { };
    }

    isOpen() {
        return this.props.open === true;
    }

    render() {
        return (
            <div
                className={'modal fade' + (this.isOpen() ? ' show' : '')}
                tabIndex="-1"
                style={{ display: this.isOpen() ? 'block' : 'none' }}
                aria-hidden={this.isOpen() ? undefined : 'true'}
                aria-modal={this.isOpen() ? 'true' : undefined}
                role={this.isOpen() ? 'dialog' : undefined}
            >
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h1 className="modal-title fs-5">test</h1>
                            <button type="button" className="btn-close" />
                        </div>
                        <div className="modal-body">
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary">Close</button>
                            <button type="button" className="btn btn-primary">Abc</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default DeleteFormModal;