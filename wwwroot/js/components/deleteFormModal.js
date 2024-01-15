import React from 'react';
import { useEffect, useState } from 'react';
import Modal from './modal.js';
import { hasStringValue } from './utils.js';

/**
 * Modal used to confirm deletion of form and checking if form can be deleted. Then modal is opened, it receives current errors preventing deletion in props (if it can't be deleted),
 * and makes an backend call to check if it can be deleted, so an up to date error list can be displayed.
 * @param {Object} props
 * @param {boolean} props.open Optional, default = false
 * @param {string} props.url
 * @param {string} props.id
 * @param {string} props.name Name of the object being delete
 * @param {string} props.title Category title
 * @param {Array<string>} props.deleteErrors
 * @param {Function} props.onCloseClick
 * @param {Function} props.onDelete Called after successful deletion
 * @param {Function} props.onError Called after an error occurs in deletion
 * @returns {JSX.Element}
 */
export default function DeleteFormModal(props) {
    //State
    const [currentId, setCurrentId] = useState('');
    const [errors, setErrors] = useState([]);

    //Form id change listener
    useEffect(() => {
        if (hasStringValue(props.id) && props.id !== currentId) {
            //Id has changed, check if this form can be deleted
            //Set state
            setCurrentId(props.id);
            setErrors(props.deleteErrors);

            //Backend call
            fetch(baseUrl + props.url + '/DeleteCheck?id=' + props.id)
                .then(response => {
                    if (response.status === 200) {
                        return response.json();
                    } else if (response.status === 410) {
                        return -1;
                    } else {
                        return -2;
                    }
                })
                .then(data => {
                    if (data === -1) {
                        //Already deleted (or never existed :\)
                        setErrors([localization.AlreadyDeleted]);
                    } else if (data === -2) {
                        //Unknown error
                        setErrors([localization.GenericErrorMessage]);
                    } else {
                        setErrors(Array.isArray(data.errors) ? data.errors : []);
                    }
                });
        }
    }, [props.id])

    function handleDeleteClick() {
        if (errors.length === 0) {
            props.onCloseClick();
            var id = currentId;
            fetch(baseUrl + props.url + '?id=' + id, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.status === 200) {
                        //Success
                        return null;
                    } else {
                        //Error, check if there are messages in response
                        return response.headers.get("content-type") !== null && response.headers.get("content-type").indexOf('application/json') !== -1 ?
                            response.json() : localization.GenericErrorMessage;
                    }
                })
                .then(data => {
                    if (data !== null) {
                        //Show error message
                        if (typeof data === 'string') {
                            props.onError([data]);
                        } else if (Array.isArray(data.errors)) {
                            props.onError(data.errors);
                        } else {
                            props.onError([localization.GenericErrorMessage]);
                        }
                    } else {
                        props.onDelete(id);
                    }
                });
        }
    }

    return (
        <Modal
            title={localization.ConfirmDeleteModalTitle.replace('[name]', props.title.toLowerCase())}
            open={props.open === true}
            onCloseClick={props.onCloseClick}
            closeButtonText={localization.Cancel}
            buttons={[{
                text: localization.Delete,
                onClick: handleDeleteClick,
                disabled: errors.length !== 0,
                className: 'btn-danger'
            }]}
        >
            <p>{localization.ConfirmDeleteMessage.replace('[name]', props.name)}</p>
            {
                errors.length === 0 ?
                    null :
                    <>
                        <p style={{ marginBottom: '5px', fontWeight: 700, color: '#ff2626' }}>{localization.CantBeDeletedMessage}</p>
                        <ul>
                            {errors.map(error => { return ( <li>{error}</li> ); })}
                        </ul>
                    </>
            }
        </Modal>
    );
}
